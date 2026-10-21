using System.Globalization;
using System.Text;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    /// <summary>
    /// Создание учётных записей и связанных с ними бизнес-сущностей.
    /// Самостоятельной регистрации пользователей в системе нет.
    /// </summary>
    [Authorize(Roles = "admin,moderator")]
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _identityDb;
        private readonly AnketaContext _anketaDb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegistrationController> _logger;

        private const int MaxCsvBytes = 10 * 1024 * 1024;

        public RegistrationController(
            ApplicationDbContext identityDb,
            AnketaContext anketaDb,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegistrationController> logger)
        {
            _identityDb = identityDb;
            _anketaDb = anketaDb;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegistrationPageModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationPageModel model)
        {
            model.Role = Normalize(model.Role);
            model.TestableMode = Normalize(model.TestableMode);
            model.Delimiter = model.Delimiter?.Trim() ?? string.Empty;

            if (model.Role == RegistrationRoles.TestableUser &&
                model.TestableMode != TestableRegistrationModes.Existing &&
                model.TestableMode != TestableRegistrationModes.New)
            {
                model.TestableMode = TestableRegistrationModes.Existing;
            }

            var allowedRoles = new[]
            {
                RegistrationRoles.Admin,
                RegistrationRoles.Moderator,
                RegistrationRoles.Verificator,
                RegistrationRoles.TestableUser,
                RegistrationRoles.FilialUser
            };

            if (!allowedRoles.Contains(model.Role, StringComparer.Ordinal))
                model.Errors.Add(new RegistrationErrorModel { Message = "Недопустимая роль пользователя." });

            if (model.Delimiter.Length != 1)
                model.Errors.Add(new RegistrationErrorModel { Message = "Разделитель должен содержать ровно один символ." });

            string? source = null;
            if (model.CsvFile != null && model.CsvFile.Length > 0 && !string.IsNullOrWhiteSpace(model.TextData))
            {
                model.Errors.Add(new RegistrationErrorModel { Message = "Укажите либо текстовые данные, либо CSV-файл, но не оба источника одновременно." });
            }
            else if (model.CsvFile != null && model.CsvFile.Length > 0)
            {
                if (model.CsvFile.Length > MaxCsvBytes)
                    model.Errors.Add(new RegistrationErrorModel { Message = $"CSV-файл слишком большой. Максимальный размер: {MaxCsvBytes / 1024 / 1024} МБ." });
                else
                    source = await ReadCsvAsync(model.CsvFile);
            }
            else if (!string.IsNullOrWhiteSpace(model.TextData))
            {
                source = model.TextData;
            }
            else
            {
                model.Errors.Add(new RegistrationErrorModel { Message = "Необходимо указать данные в текстовом поле или загрузить CSV-файл." });
            }

            if (model.Errors.Count > 0)
                return View("Index", model);

            try
            {
                var result = await ValidateAndRegisterAsync(model.Role, model.TestableMode, source!, model.Delimiter[0]);
                model.Errors = result.Errors;
                model.ParsedCount = result.ParsedCount;
                model.CreatedCount = result.CreatedCount;

                if (result.Errors.Count > 0)
                    return View("Index", model);

                TempData["RegistrationSuccess"] = $"Успешно зарегистрировано пользователей: {result.CreatedCount}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка массовой регистрации пользователей. Role={Role}, Mode={Mode}", model.Role, model.TestableMode);
                model.Errors.Add(new RegistrationErrorModel { Message = "Регистрация не выполнена. Все изменения отменены." });
                return View("Index", model);
            }
        }

        private async Task<RegistrationExecutionResult> ValidateAndRegisterAsync(
            string role,
            string testableMode,
            string source,
            char delimiter)
        {
            return role switch
            {
                RegistrationRoles.Admin or RegistrationRoles.Moderator =>
                    await ProcessAsync(
                        ParseRows(source, delimiter, 2, (parts, _) => new LoginDataModel
                        {
                            Login = parts[0],
                            Password = parts[1]
                        }),
                        role),

                RegistrationRoles.Verificator or RegistrationRoles.FilialUser =>
                    await ProcessAsync(
                        ParseRows(source, delimiter, 3, (parts, _) => new LoginFilialDataModel
                        {
                            Login = parts[0],
                            Password = parts[1],
                            FilialName = parts[2]
                        }),
                        role),

                RegistrationRoles.TestableUser when testableMode == TestableRegistrationModes.Existing =>
                    await ProcessAsync(
                        ParseRows(source, delimiter, 3, (parts, _) => new LoginDataWithCodeModel
                        {
                            Login = parts[0],
                            Password = parts[1],
                            Code = parts[2]
                        }),
                        role),

                RegistrationRoles.TestableUser when testableMode == TestableRegistrationModes.New =>
                    await ProcessAsync(
                        ParseRows(source, delimiter, 6, (parts, _) => new LoginDataWithTestableModel
                        {
                            Login = parts[0],
                            Password = parts[1],
                            Code = parts[2],
                            FilialName = parts[3],
                            ReservLevelID = ParseInt(parts[4]),
                            TestablePeriodID = ParseInt(parts[5])
                        }),
                        role),

                _ => new RegistrationExecutionResult
                {
                    Errors = new List<RegistrationErrorModel>
                    {
                        new() { Message = "Неизвестный сценарий регистрации." }
                    }
                }
            };
        }

        private async Task<RegistrationExecutionResult> ProcessAsync<T>(
            ParsedBatch<T> parsed,
            string role)
        {
            var result = new RegistrationExecutionResult
            {
                ParsedCount = parsed.Items.Count,
                Errors = parsed.Errors
            };

            if (result.Errors.Count > 0 || parsed.Items.Count == 0)
            {
                if (parsed.Items.Count == 0 && result.Errors.Count == 0)
                    result.Errors.Add(new RegistrationErrorModel { Message = "Не найдено ни одной строки для регистрации." });
                return result;
            }

            var loginItems = parsed.Items.Select(x => new BatchLogin<T>(x.Line, GetLogin(x.Data))).ToList();

            // Дубликаты внутри входного пакета.
            var duplicateGroups = loginItems
                .GroupBy(x => x.Login, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateGroups)
            {
                foreach (var item in group)
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = item.Login,
                        Message = $"Логин '{item.Login}' повторяется в загружаемом наборе."
                    });
                }
            }

            var normalizedLogins = loginItems
                .Select(x => _userManager.NormalizeName(x.Login) ?? x.Login.ToUpperInvariant())
                .Distinct(StringComparer.Ordinal)
                .ToList();

            var existingLogins = await _identityDb.Users
                .AsNoTracking()
                .Where(u => u.NormalizedUserName != null && normalizedLogins.Contains(u.NormalizedUserName!))
                .Select(u => u.NormalizedUserName!)
                .ToListAsync();

            var existingLoginSet = existingLogins.ToHashSet(StringComparer.Ordinal);
            foreach (var item in loginItems)
            {
                var normalized = _userManager.NormalizeName(item.Login) ?? item.Login.ToUpperInvariant();
                if (existingLoginSet.Contains(normalized))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = item.Login,
                        Message = $"Логин '{item.Login}' уже существует в системе."
                    });
                }

                if (string.IsNullOrWhiteSpace(item.Login))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Message = "Логин не может быть пустым."
                    });
                }
                else if (item.Login.Length > 256)
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = item.Login,
                        Message = "Логин не может быть длиннее 256 символов."
                    });
                }
            }

            switch (role)
            {
                case RegistrationRoles.Admin:
                case RegistrationRoles.Moderator:
                    await ValidatePasswordsAsync(parsed.Items, result);
                    await ValidateRoleExistsAsync(role, result);
                    break;

                case RegistrationRoles.Verificator:
                case RegistrationRoles.FilialUser:
                    await ValidatePasswordsAsync(parsed.Items, result);
                    await ValidateRoleExistsAsync(role, result);
                    await ValidateFilialsAsync(parsed.Items, result);
                    await ValidateNoUserFilialOrphansAsync(parsed.Items.Select(x => GetLogin(x.Data)).ToList(), result);
                    break;

                case RegistrationRoles.TestableUser:
                    await ValidatePasswordsAsync(parsed.Items, result);
                    await ValidateRoleExistsAsync(role, result);
                    break;
            }

            if (role == RegistrationRoles.TestableUser)
            {
                if (parsed.Items.First().Data is LoginDataWithCodeModel)
                    await ValidateExistingTestablesAsync(parsed.Items, result);
                else if (parsed.Items.First().Data is LoginDataWithTestableModel)
                    await ValidateNewTestablesAsync(parsed.Items, result);
            }

            if (result.Errors.Count > 0)
                return result;

            return await RegisterAllAsync(parsed.Items, role);
        }

        private async Task ValidatePasswordsAsync<T>(List<ParsedItem<T>> items, RegistrationExecutionResult result)
        {
            foreach (var item in items)
            {
                var (login, password) = GetCredentials(item.Data);
                if (string.IsNullOrWhiteSpace(password))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = login,
                        Message = "Пароль не может быть пустым."
                    });
                    continue;
                }

                var tempUser = new IdentityUser { UserName = login };
                foreach (var validator in _userManager.PasswordValidators)
                {
                    var validation = await validator.ValidateAsync(_userManager, tempUser, password);
                    if (!validation.Succeeded)
                    {
                        foreach (var error in validation.Errors)
                        {
                            result.Errors.Add(new RegistrationErrorModel
                            {
                                Line = item.Line,
                                Login = login,
                                Message = error.Description
                            });
                        }
                    }
                }
            }
        }

        private async Task ValidateRoleExistsAsync(string role, RegistrationExecutionResult result)
        {
            if (await _roleManager.FindByNameAsync(role) == null)
                result.Errors.Add(new RegistrationErrorModel { Message = $"Роль '{role}' отсутствует в Identity DB." });
        }

        private async Task ValidateFilialsAsync<T>(List<ParsedItem<T>> items, RegistrationExecutionResult result)
        {
            var filialNames = items
                .Select(x => GetFilialName(x.Data))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var filials = await _anketaDb.Filials
                .AsNoTracking()
                .Select(f => new { f.ID, f.FilialName })
                .ToListAsync();

            var filialMap = filials.ToDictionary(f => f.FilialName, f => f.ID, StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                var name = GetFilialName(item.Data)?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = GetLogin(item.Data),
                        Message = "Не указан филиал."
                    });
                    continue;
                }

                if (!filialMap.ContainsKey(name))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = GetLogin(item.Data),
                        Message = $"Филиал '{name}' не найден."
                    });
                }
            }
        }

        private async Task ValidateNoUserFilialOrphansAsync(List<string> logins, RegistrationExecutionResult result)
        {
            var set = logins.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existing = await _anketaDb.UserFilials
                .AsNoTracking()
                .Where(x => x.UserName != null)
                .Select(x => x.UserName!)
                .ToListAsync();

            foreach (var login in existing.Where(x => set.Contains(x)))
            {
                result.Errors.Add(new RegistrationErrorModel
                {
                    Login = login,
                    Message = $"В Anketa DB уже существует привязка UserFilial для логина '{login}'."
                });
            }
        }

        private async Task ValidateExistingTestablesAsync<T>(List<ParsedItem<T>> items, RegistrationExecutionResult result)
        {
            var codes = items.Select(x => GetCode(x.Data)?.Trim() ?? string.Empty).ToList();
            if (codes.Any(string.IsNullOrWhiteSpace))
            {
                foreach (var item in items.Where(x => string.IsNullOrWhiteSpace(GetCode(x.Data))))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = GetLogin(item.Data), Message = "Код тестируемого не указан." });
            }

            var testables = await _anketaDb.Testables
                .AsNoTracking()
                .Where(t => t.Code != null)
                .Select(t => new { t.ID, t.Code })
                .ToListAsync();

            var testableByCode = testables
                .Where(t => t.Code != null)
                .ToDictionary(t => t.Code, t => t.ID, StringComparer.OrdinalIgnoreCase);

            var existingTestableBindings = await _anketaDb.TestableUsers
                .AsNoTracking()
                .Where(x => x.UserName != null)
                .Select(x => new { x.UserName, TestableId = x.testable != null ? (int?)x.testable.ID : null })
                .ToListAsync();

            var occupiedTestableIds = existingTestableBindings
                .Where(x => x.TestableId.HasValue)
                .Select(x => x.TestableId!.Value)
                .ToHashSet();

            var occupiedTestableUserNames = existingTestableBindings
                .Where(x => x.UserName != null)
                .Select(x => x.UserName!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                var code = GetCode(item.Data)?.Trim() ?? string.Empty;
                if (!testableByCode.TryGetValue(code, out var testableId))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = GetLogin(item.Data),
                        Message = $"Тестируемый с кодом '{code}' не найден."
                    });
                    continue;
                }

                var login = GetLogin(item.Data);
                if (occupiedTestableIds.Contains(testableId))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = login,
                        Message = $"Тестируемый с кодом '{code}' уже привязан к другому аккаунту."
                    });
                }

                if (occupiedTestableUserNames.Contains(login))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = login,
                        Message = $"Логин '{login}' уже имеет привязку TestableUser в Anketa DB."
                    });
                }
            }

            // Заодно защищаемся от дубликата самого кода внутри пакета.
            var duplicateCodes = codes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateCodes)
            {
                foreach (var item in items.Where(x => string.Equals(GetCode(x.Data)?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = GetLogin(item.Data),
                        Message = $"Код тестируемого '{group.Key}' повторяется в загружаемом наборе."
                    });
                }
            }
        }

        private async Task ValidateNewTestablesAsync<T>(List<ParsedItem<T>> items, RegistrationExecutionResult result)
        {
            var codes = items.Select(x => GetCode(x.Data)?.Trim() ?? string.Empty).ToList();
            var duplicateCodes = codes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var group in duplicateCodes)
            {
                foreach (var item in items.Where(x => string.Equals(GetCode(x.Data)?.Trim(), group.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Errors.Add(new RegistrationErrorModel
                    {
                        Line = item.Line,
                        Login = GetLogin(item.Data),
                        Message = $"Код нового тестируемого '{group.Key}' повторяется в загружаемом наборе."
                    });
                }
            }

            var existingCodes = await _anketaDb.Testables
                .AsNoTracking()
                .Select(t => t.Code)
                .ToListAsync();
            var existingCodeSet = existingCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingTestableUserNames = (await _anketaDb.TestableUsers
                .AsNoTracking()
                .Where(x => x.UserName != null)
                .Select(x => x.UserName!)
                .ToListAsync())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                var data = item.Data as LoginDataWithTestableModel;
                if (data == null) continue;

                if (string.IsNullOrWhiteSpace(data.Code))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = "Код тестируемого не указан." });
                else if (existingCodeSet.Contains(data.Code.Trim()))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = $"Код '{data.Code.Trim()}' уже существует." });

                if (existingTestableUserNames.Contains(data.Login.Trim()))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = $"Логин '{data.Login.Trim()}' уже имеет привязку TestableUser в Anketa DB." });

                if (string.IsNullOrWhiteSpace(data.FilialName))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = "Филиал не указан." });

                if (data.ReservLevelID <= 0)
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = "ReservLevelID должен быть положительным." });

                if (data.TestablePeriodID <= 0)
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = "TestablePeriodID должен быть положительным." });
            }

            var filialMap = (await _anketaDb.Filials.AsNoTracking().ToListAsync())
                .ToDictionary(x => x.FilialName, x => x, StringComparer.OrdinalIgnoreCase);

            foreach (var item in items)
            {
                var data = item.Data as LoginDataWithTestableModel;
                if (data == null) continue;
                if (!string.IsNullOrWhiteSpace(data.FilialName) && !filialMap.ContainsKey(data.FilialName.Trim()))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = $"Филиал '{data.FilialName.Trim()}' не найден." });
            }

            var reservIds = items
                .Select(x => (x.Data as LoginDataWithTestableModel)?.ReservLevelID ?? 0)
                .Where(x => x > 0)
                .Distinct()
                .ToList();
            var existingReservIds = (await _anketaDb.ReservLevels.AsNoTracking().Where(x => reservIds.Contains(x.ID)).Select(x => x.ID).ToListAsync()).ToHashSet();
            foreach (var item in items)
            {
                var data = item.Data as LoginDataWithTestableModel;
                if (data == null) continue;
                if (data.ReservLevelID > 0 && !existingReservIds.Contains(data.ReservLevelID))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = $"Уровень резерва ID={data.ReservLevelID} не найден." });
            }

            var periodIds = items
                .Select(x => (x.Data as LoginDataWithTestableModel)?.TestablePeriodID ?? 0)
                .Where(x => x > 0)
                .Distinct()
                .ToList();
            var existingPeriodIds = (await _anketaDb.TestPeriods.AsNoTracking().Where(x => periodIds.Contains(x.ID)).Select(x => x.ID).ToListAsync()).ToHashSet();
            foreach (var item in items)
            {
                var data = item.Data as LoginDataWithTestableModel;
                if (data == null) continue;
                if (data.TestablePeriodID > 0 && !existingPeriodIds.Contains(data.TestablePeriodID))
                    result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = data.Login, Message = $"Период ID={data.TestablePeriodID} не найден." });
            }
        }

        private async Task<RegistrationExecutionResult> RegisterAllAsync<T>(List<ParsedItem<T>> items, string role)
        {
            var result = new RegistrationExecutionResult { ParsedCount = items.Count };
            var createdUsers = new List<IdentityUser>();
            var identityCommitted = false;

            await using var anketaTransaction = await _anketaDb.Database.BeginTransactionAsync();
            await using var identityTransaction = await _identityDb.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in items)
                {
                    var (login, password) = GetCredentials(item.Data);
                    var user = new IdentityUser { UserName = login.Trim() };
                    var createResult = await _userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        foreach (var error in createResult.Errors)
                            result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = login, Message = error.Description });
                        throw new InvalidOperationException("Не удалось создать одного из пользователей Identity.");
                    }

                    createdUsers.Add(user);

                    var roleResult = await _userManager.AddToRoleAsync(user, role);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                            result.Errors.Add(new RegistrationErrorModel { Line = item.Line, Login = login, Message = error.Description });
                        throw new InvalidOperationException("Не удалось назначить роль одному из пользователей.");
                    }

                    switch (item.Data)
                    {
                        case LoginFilialDataModel filialModel:
                            var filial = await FindFilialAsync(filialModel.FilialName);
                            if (filial == null)
                                throw new InvalidOperationException($"Филиал '{filialModel.FilialName}' не найден при сохранении.");

                            _anketaDb.UserFilials.Add(new UserFilial
                            {
                                UserName = login.Trim(),
                                Filial = filial
                            });
                            break;

                        case LoginDataWithCodeModel codeModel:
                            var existingTestable = await _anketaDb.Testables.FirstOrDefaultAsync(t => t.Code == codeModel.Code.Trim());
                            if (existingTestable == null)
                                throw new InvalidOperationException($"Тестируемый с кодом '{codeModel.Code.Trim()}' не найден при сохранении.");

                            _anketaDb.TestableUsers.Add(new TestableUser
                            {
                                UserName = login.Trim(),
                                testable = existingTestable,
                                LK_Data = null
                            });
                            break;

                        case LoginDataWithTestableModel newModel:
                            var newFilial = await FindFilialAsync(newModel.FilialName);
                            var reservLevel = await _anketaDb.ReservLevels.FirstOrDefaultAsync(r => r.ID == newModel.ReservLevelID);
                            var period = await _anketaDb.TestPeriods.FirstOrDefaultAsync(p => p.ID == newModel.TestablePeriodID);

                            if (newFilial == null || reservLevel == null || period == null)
                                throw new InvalidOperationException("Одна из связанных сущностей была удалена после предварительной проверки.");

                            var testable = new Testable
                            {
                                Code = newModel.Code.Trim(),
                                YearTraining = DateTime.Now.Year,
                                isArchived = false,
                                filial = newFilial,
                                reservLevel = reservLevel
                            };

                            _anketaDb.Testables.Add(testable);
                            _anketaDb.TestableUsers.Add(new TestableUser
                            {
                                UserName = login.Trim(),
                                testable = testable,
                                LK_Data = null
                            });
                            _anketaDb.AccessForTestables.Add(new AccessForTestable
                            {
                                Testable = testable,
                                TestPeriod = period,
                                isActive = true
                            });
                            break;
                    }
                }

                await _anketaDb.SaveChangesAsync();

                // Сначала коммитим Identity. Если после этого бизнес-транзакция не сможет
                // завершиться, созданные аккаунты будут компенсированы ниже.
                await identityTransaction.CommitAsync();
                identityCommitted = true;
                await anketaTransaction.CommitAsync();

                result.CreatedCount = items.Count;
                return result;
            }
            catch
            {
                try
                {
                    await anketaTransaction.RollbackAsync();
                }
                catch (Exception rollbackException)
                {
                    _logger.LogError(rollbackException, "Не удалось откатить транзакцию Anketa DB при регистрации.");
                }

                if (!identityCommitted)
                {
                    try
                    {
                        await identityTransaction.RollbackAsync();
                    }
                    catch (Exception rollbackException)
                    {
                        _logger.LogError(rollbackException, "Не удалось откатить транзакцию Identity DB при регистрации.");
                    }
                }
                else
                {
                    // Identity уже зафиксирован, а бизнес-транзакция завершилась ошибкой.
                    // Компенсируем созданные аккаунты отдельной транзакцией.
                    await CompensateIdentityUsersAsync(createdUsers);
                }

                result.CreatedCount = 0;
                if (result.Errors.Count == 0)
                    result.Errors.Add(new RegistrationErrorModel { Message = "Регистрация не выполнена. Все изменения отменены." });
                return result;
            }
        }


        private async Task CompensateIdentityUsersAsync(List<IdentityUser> users)
        {
            if (users.Count == 0) return;

            await using var compensationTransaction = await _identityDb.Database.BeginTransactionAsync();
            try
            {
                foreach (var user in users)
                {
                    var currentUser = await _userManager.FindByIdAsync(user.Id);
                    if (currentUser == null) continue;

                    var deleteResult = await _userManager.DeleteAsync(currentUser);
                    if (!deleteResult.Succeeded)
                    {
                        var details = string.Join("; ", deleteResult.Errors.Select(x => x.Description));
                        throw new InvalidOperationException($"Не удалось удалить аккаунт '{currentUser.UserName}' при откате: {details}");
                    }
                }

                await compensationTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try { await compensationTransaction.RollbackAsync(); } catch { }
                _logger.LogError(ex, "Ошибка компенсирующего удаления пользователей Identity после сбоя регистрации.");
                throw;
            }
        }

        private async Task<Filial?> FindFilialAsync(string? filialName)
        {
            if (string.IsNullOrWhiteSpace(filialName)) return null;
            var name = filialName.Trim();
            var normalizedName = name.ToLower();
            return await _anketaDb.Filials.FirstOrDefaultAsync(f => f.FilialName.ToLower() == normalizedName);
        }

        private static async Task<string> ReadCsvAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync();
        }

        private static ParsedBatch<T> ParseRows<T>(
            string source,
            char delimiter,
            int expectedColumns,
            Func<string[], int, T> factory)
        {
            var batch = new ParsedBatch<T>();
            var lines = source.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            var firstDataLineSkipped = false;

            for (var index = 0; index < lines.Length; index++)
            {
                var lineNumber = index + 1;
                var line = lines[index];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(delimiter);
                if (!firstDataLineSkipped && LooksLikeHeader(parts))
                {
                    firstDataLineSkipped = true;
                    continue;
                }
                firstDataLineSkipped = true;

                if (parts.Length != expectedColumns)
                {
                    batch.Errors.Add(new RegistrationErrorModel
                    {
                        Line = lineNumber,
                        Message = $"Ожидается {expectedColumns} полей, получено {parts.Length}."
                    });
                    continue;
                }

                parts = parts.Select(x => x.Trim()).ToArray();
                try
                {
                    batch.Items.Add(new ParsedItem<T>
                    {
                        Line = lineNumber,
                        Data = factory(parts, lineNumber)
                    });
                }
                catch (Exception ex)
                {
                    batch.Errors.Add(new RegistrationErrorModel
                    {
                        Line = lineNumber,
                        Message = $"Ошибка разбора строки: {ex.Message}"
                    });
                }
            }

            return batch;
        }

        private static bool LooksLikeHeader(string[] parts)
        {
            if (parts.Length == 0) return false;
            var first = parts[0].Trim().ToLowerInvariant();
            return first is "login" or "username" or "user_name";
        }

        private static int ParseInt(string value)
        {
            if (int.TryParse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
                return result;
            return 0;
        }

        private static string Normalize(string? value) => value?.Trim() ?? string.Empty;

        private static string GetLogin<T>(T data) => data switch
        {
            LoginDataModel x => x.Login?.Trim() ?? string.Empty,
            LoginFilialDataModel x => x.Login?.Trim() ?? string.Empty,
            LoginDataWithCodeModel x => x.Login?.Trim() ?? string.Empty,
            LoginDataWithTestableModel x => x.Login?.Trim() ?? string.Empty,
            _ => string.Empty
        };

        private static string GetFilialName<T>(T data) => data switch
        {
            LoginFilialDataModel x => x.FilialName?.Trim() ?? string.Empty,
            LoginDataWithTestableModel x => x.FilialName?.Trim() ?? string.Empty,
            _ => string.Empty
        };

        private static string GetCode<T>(T data) => data switch
        {
            LoginDataWithCodeModel x => x.Code?.Trim() ?? string.Empty,
            LoginDataWithTestableModel x => x.Code?.Trim() ?? string.Empty,
            _ => string.Empty
        };

        private static (string Login, string Password) GetCredentials<T>(T data) => data switch
        {
            LoginDataModel x => (x.Login?.Trim() ?? string.Empty, x.Password ?? string.Empty),
            LoginFilialDataModel x => (x.Login?.Trim() ?? string.Empty, x.Password ?? string.Empty),
            LoginDataWithCodeModel x => (x.Login?.Trim() ?? string.Empty, x.Password ?? string.Empty),
            LoginDataWithTestableModel x => (x.Login?.Trim() ?? string.Empty, x.Password ?? string.Empty),
            _ => (string.Empty, string.Empty)
        };

        private sealed class ParsedBatch<T>
        {
            public List<ParsedItem<T>> Items { get; } = new();
            public List<RegistrationErrorModel> Errors { get; } = new();
        }

        private sealed class ParsedItem<T>
        {
            public int Line { get; set; }
            public T Data { get; set; } = default!;
        }

        private sealed class BatchLogin<T>
        {
            public BatchLogin(int line, string login)
            {
                Line = line;
                Login = login;
            }

            public int Line { get; }
            public string Login { get; }
        }

        private sealed class RegistrationExecutionResult
        {
            public List<RegistrationErrorModel> Errors { get; set; } = new();
            public int ParsedCount { get; set; }
            public int CreatedCount { get; set; }
        }
    }
}
