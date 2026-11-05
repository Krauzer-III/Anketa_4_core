using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using static Anketa_4_core.Data.AnketaModels.JSON_Motivation_Result;

namespace Anketa_4_core.Controllers
{
    [Authorize]
    public class KettelController : Controller
    {
        private readonly AnketaContext _db;
        private readonly UserManager<IdentityUser> _users;

        public KettelController(AnketaContext db, UserManager<IdentityUser> users)
        {
            _db = db;
            _users = users;
        }

        // Личная учётная запись сразу переходит к анкете;
        // общий филиальный логин сначала вводит код тестируемого.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            if (await _users.IsInRoleAsync(user, "testable_user"))
            {
                var linked = await _db.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable == null)
                    return TestError("К личной учётной записи не привязан тестируемый.");

                var latest = await _db.AccessForTestables
                    .Where(x => x.Testable.ID == linked.testable.ID)
                    .OrderByDescending(x => x.ID).FirstOrDefaultAsync();
                if (latest == null) return TestError("Тестируемому не назначен период тестирования.");

                var (access, error) = await CheckAccess(latest.ID, user);
                if (access == null) return TestError(error!);
                if (await IsCompleted(access.ID))
                    return TestError("Психологическое тестирование за текущий период уже пройдено.");

                return RedirectToAction(nameof(Start), new { accessId = access.ID });
            }

            return View(new MVC_KettelModel_Login());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MVC_KettelModel_Login model)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            if (await _users.IsInRoleAsync(user, "testable_user"))
                return RedirectToAction(nameof(Index));
            if (!ModelState.IsValid) return View(model);

            var testable = await _db.Testables.FirstOrDefaultAsync(x => x.Code == model.Code);
            if (testable == null)
            {
                ModelState.AddModelError(nameof(model.Code), "Такого кода не существует.");
                return View(model);
            }

            var latest = await _db.AccessForTestables
                .Where(x => x.Testable.ID == testable.ID)
                .OrderByDescending(x => x.ID).FirstOrDefaultAsync();
            if (latest == null)
            {
                ModelState.AddModelError("", "Тестируемому не назначен период тестирования.");
                return View(model);
            }

            var (access, error) = await CheckAccess(latest.ID, user);
            if (access == null)
            {
                ModelState.AddModelError("", error!);
                return View(model);
            }

            if (await IsCompleted(access.ID))
            {
                ModelState.AddModelError("", "Психологическое тестирование за текущий период уже пройдено.");
                return View(model);
            }

            return RedirectToAction(nameof(Start), new { accessId = access.ID });
        }

        private async Task<(AccessForTestable? access, string? error)> CheckAccess(
            int accessId, IdentityUser user)
        {
            var access = await _db.AccessForTestables
                .Include(x => x.Testable).ThenInclude(x => x.filial)
                .FirstOrDefaultAsync(x => x.ID == accessId);
            if (access == null)
                return (null, "Период тестирования не найден.");

            bool personal = await _users.IsInRoleAsync(user, "testable_user");
            if (personal)
            {
                var linked = await _db.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable?.ID != access.Testable.ID)
                    return (null, "У личной учётной записи нет доступа к этому тестируемому.");
            }
            else
            {
                var filial = await _db.UserFilials.Include(x => x.Filial)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (filial?.Filial == null)
                    return (null, "Учётной записи не назначен филиал.");
                if (filial.Filial.ID != access.Testable.filial.ID)
                    return (null, "Код тестируемого не относится к вашему филиалу.");
            }

            // Последний назначенный период, а не последний среди активных.
            var latest = await _db.AccessForTestables
                .Where(x => x.Testable.ID == access.Testable.ID)
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, x.isActive })
                .FirstOrDefaultAsync();
            if (latest == null || latest.ID != access.ID)
                return (null, "Этот период тестирования больше не является последним.");
            if (!latest.isActive)
                return (null, "Последний период тестирования закрыт.");

            return (access, null);
        }

        private async Task<bool> IsCompleted(int accessId)
        {
            var questions = await _db.KT_Questions.Select(x => x.ID).ToArrayAsync();
            if (questions.Length == 0) return false;

            var answered = await _db.KT_TestableAnswers
                .Where(x => x.MainInfo.Access.ID == accessId)
                .Select(x => x.Answer.Question.ID)
                .Distinct().ToArrayAsync();
            return questions.All(answered.Contains);
        }

        [HttpGet]
        public async Task<IActionResult> Start(int accessId)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(accessId, user);
            if (access == null) return TestError(error!);
            if (!await _db.KT_Questions.AnyAsync())
                return TestError("Вопросы психологического тестирования ещё не загружены.");
            if (await IsCompleted(accessId))
                return TestError("Психологическое тестирование за текущий период уже пройдено.");

            var main = await _db.KT_Mains.FirstOrDefaultAsync(x => x.Access.ID == accessId);
            if (main != null)
                return RedirectToAction(nameof(Question), new { accessId });

            return View(new MVC_KettelModel_Main { AccessID = accessId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(MVC_KettelModel_Main model)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(model.AccessID, user);
            if (access == null) return TestError(error!);

            if (model.Gender != "Мужчина" && model.Gender != "Женщина")
                ModelState.AddModelError(nameof(model.Gender), "Выберите пол.");
            if (!ModelState.IsValid) return View(model);

            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await LockAccess(access.ID);

            var existing = await _db.KT_Mains.FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            if (existing == null)
            {
                _db.KT_Mains.Add(new KT_Main
                {
                    Access = access,
                    Years = model.Years,
                    Gender = model.Gender
                });
                await _db.SaveChangesAsync();
            }
            await tx.CommitAsync();

            return RedirectToAction(nameof(Question), new { accessId = access.ID });
        }

        // Номер вопроса не передаётся из браузера: показывается первый
        // вопрос из KT_Questions.Number, у которого ещё нет ответа.
        [HttpGet]
        public async Task<IActionResult> Question(int accessId)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(accessId, user);
            if (access == null) return TestError(error!);

            var main = await _db.KT_Mains.FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            if (main == null)
                return RedirectToAction(nameof(Start), new { accessId });

            var questions = await _db.KT_Questions.AsNoTracking()
                .OrderBy(x => x.Number).ThenBy(x => x.ID).ToArrayAsync();
            if (questions.Length == 0)
                return TestError("Вопросы психологического тестирования не загружены.");

            var answered = await _db.KT_TestableAnswers
                .Where(x => x.MainInfo.ID == main.ID)
                .Select(x => x.Answer.Question.ID).Distinct().ToArrayAsync();

            var question = questions.FirstOrDefault(x => !answered.Contains(x.ID));
            if (question == null)
                return RedirectToAction(nameof(Finish), new { accessId });

            var answers = await _db.KT_Answers.AsNoTracking()
                .Where(x => x.Question.ID == question.ID)
                .OrderBy(x => x.AnswerNumber).ThenBy(x => x.ID)
                .Select(x => new MVC_KettelModel_Answer
                {
                    ID = x.ID,
                    AnswerNumber = x.AnswerNumber,
                    AnswerText = x.AnswerText
                }).ToArrayAsync();
            if (answers.Length == 0)
                return TestError($"Для вопроса {question.Number} не заданы варианты ответов.");

            return View(new MVC_KettelModel_Question
            {
                AccessID = access.ID,
                Number = question.Number,
                Position = Array.IndexOf(questions, question) + 1,
                TotalQuestions = questions.Length,
                Text = question.Text,
                Answers = answers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Question(MVC_KettelModel_Submit model)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(model.AccessID, user);
            if (access == null) return TestError(error!);
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Question), new { accessId = access.ID });

            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await LockAccess(access.ID);

            // После блокировки повторяем проверку периода и ответа, чтобы
            // двойное нажатие/две вкладки не создали дубль.
            var latest = await _db.AccessForTestables
                .Where(x => x.Testable.ID == access.Testable.ID)
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, x.isActive }).FirstOrDefaultAsync();
            if (latest == null || latest.ID != access.ID || !latest.isActive)
                return TestError("Период тестирования изменён или закрыт.");

            var main = await _db.KT_Mains.FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            if (main == null)
                return TestError("Не найдена анкета. Сначала укажите возраст и пол.");

            var questions = await _db.KT_Questions
                .OrderBy(x => x.Number).ThenBy(x => x.ID).ToArrayAsync();
            if (questions.Length == 0)
                return TestError("Вопросы психологического тестирования не загружены.");

            var answeredIds = await _db.KT_TestableAnswers
                .Where(x => x.MainInfo.ID == main.ID)
                .Select(x => x.Answer.Question.ID).Distinct().ToArrayAsync();
            var next = questions.FirstOrDefault(x => !answeredIds.Contains(x.ID));
            if (next == null)
            {
                await tx.CommitAsync();
                return RedirectToAction(nameof(Finish), new { accessId = access.ID });
            }

            var selected = await _db.KT_Answers.Include(x => x.Question)
                .FirstOrDefaultAsync(x => x.ID == model.SelectedAnswerID);
            if (selected == null || selected.Question.ID != next.ID)
            {
                // При повторной отправке устаревшей формы показываем текущий вопрос.
                await tx.CommitAsync();
                return RedirectToAction(nameof(Question), new { accessId = access.ID });
            }

            var answer = new KT_TestableAnswers
            {
                MainInfo = main,
                Answer = selected,
                DateTimeAnswer = DateTime.UtcNow
            };
            _db.KT_TestableAnswers.Add(answer);
            await _db.SaveChangesAsync();

            // Не перезаписываем остальные разделы JSON_ResultModel.
            var stored = await _db.TestResults.Include(x => x.Access)
                .FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            JSON_ResultModel aggregate = stored == null
                ? new JSON_ResultModel { AccessID = access.ID, TestableCode = access.Testable.Code }
                : JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)
                    ?? throw new InvalidOperationException("Не удалось прочитать общий JSON результатов.");

            if (answeredIds.Length + 1 == questions.Length)
            {
                // Последний ответ: берём все сохранённые ответы и пересчитываем
                // сырые баллы по KT_Keys и готовые оценки по KT_Marks.
                var allAnswers = await _db.KT_TestableAnswers
                    .Include(x => x.Answer).ThenInclude(x => x.Question)
                    .Where(x => x.MainInfo.ID == main.ID)
                    .OrderBy(x => x.Answer.Question.Number).ThenBy(x => x.ID)
                    .ToArrayAsync();
                var categories = await _db.KT_Categories.ToArrayAsync();
                var keys = await _db.KT_Keys
                    .Include(x => x.Category).Include(x => x.Answer).ToArrayAsync();
                var marks = await _db.KT_Marks
                    .Include(x => x.Category).ToArrayAsync();

                try
                {
                    aggregate.KettelTest = new JSON_Kettel(main, allAnswers, categories, keys, marks);
                }
                catch (InvalidOperationException ex)
                {
                    // При ошибке таблицы нормирования последний ответ откатится
                    // вместе с обновлением JSON. Пользователь сможет повторить
                    // отправку после исправления справочников.
                    return TestError("Не удалось рассчитать результаты Кеттела: " + ex.Message);
                }
            }
            else
            {
                aggregate.Add_JSON_Kettel_Answer(main, answer);
            }

            if (stored == null)
                _db.TestResults.Add(new TestResult { Access = access, Results = aggregate.GetJSON() });
            else
                stored.Results = aggregate.GetJSON();

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            if (answeredIds.Length + 1 == questions.Length)
                return RedirectToAction(nameof(Finish), new { accessId = access.ID });

            return RedirectToAction(nameof(Question), new { accessId = access.ID });
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int accessId)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(accessId, user);
            if (access == null) return TestError(error!);

            if (!await IsCompleted(access.ID))
                return RedirectToAction(nameof(Question), new { accessId = access.ID });

            return View();
        }

        private Task LockAccess(int accessId)
        {
            // Сериализация обновлений общего TestResult.Results по AccessID.
            return _db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"AccessForTestables\" WHERE \"ID\" = {accessId} FOR UPDATE");
        }

        private IActionResult TestError(string message)
        {
            ViewBag.Error = message;
            return View("KettelError");
        }
    }
}
