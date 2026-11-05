using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Anketa_4_core.Controllers.Users
{
    /// <summary>
    /// Два независимых теста компетенций по Comp_Block.Year (1 и 2).
    /// Каждый ответ на ситуацию сохраняется сразу: 4 разных приоритета.
    /// </summary>
    [Authorize]
    public class CompetentionController : Controller
    {
        private readonly AnketaContext _db;
        private readonly UserManager<IdentityUser> _users;

        public CompetentionController(AnketaContext db, UserManager<IdentityUser> users)
        {
            _db = db;
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? year)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            bool personal = await _users.IsInRoleAsync(user, "testable_user");
            if (personal)
            {
                var linked = await _db.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable == null)
                    return TestError("К личной учётной записи не привязан тестируемый.");

                // Без явно указанного года используем год обучения.
                // Через ?year=1 или ?year=2 доступны отдельные тесты.
                int targetYear = year ?? (linked.testable.YearTraining == 2 ? 2 : 1);
                if (!ValidYear(targetYear)) return NotFound();

                var latest = await _db.AccessForTestables
                    .Where(x => x.Testable.ID == linked.testable.ID)
                    .OrderByDescending(x => x.ID).FirstOrDefaultAsync();
                if (latest == null) return TestError("Тестируемому не назначено тестирование.");

                var (access, error) = await CheckAccess(latest.ID, user);
                if (access == null) return TestError(error!);

                if (await IsCompleted(access.ID, targetYear))
                    return TestError($"Компетенции ({targetYear} год): тест уже пройден в последнем периоде.");

                return RedirectToAction(nameof(Question), new { accessId = access.ID, year = targetYear });
            }

            int selectedYear = year ?? 1;
            if (!ValidYear(selectedYear)) return NotFound();
            return View(new MVC_CompetentionLoginModel { Year = selectedYear });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MVC_CompetentionLoginModel model)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            if (await _users.IsInRoleAsync(user, "testable_user"))
                return RedirectToAction(nameof(Index), new { year = model.Year });

            if (!ValidYear(model.Year))
                ModelState.AddModelError(nameof(model.Year), "Неизвестный год тестирования.");
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

            if (await IsCompleted(access.ID, model.Year))
            {
                ModelState.AddModelError("",
                    $"Компетенции ({model.Year} год): тест уже пройден за текущий период.");
                return View(model);
            }

            return RedirectToAction(nameof(Question),
                new { accessId = access.ID, year = model.Year });
        }

        private static bool ValidYear(int year) => year == 1 || year == 2;

        private async Task<(AccessForTestable? access, string? error)> CheckAccess(
            int accessId, IdentityUser user)
        {
            var access = await _db.AccessForTestables
                .Include(x => x.Testable).ThenInclude(x => x.filial)
                .FirstOrDefaultAsync(x => x.ID == accessId);
            if (access == null) return (null, "Период тестирования не найден.");

            bool personal = await _users.IsInRoleAsync(user, "testable_user");
            if (personal)
            {
                var linked = await _db.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable?.ID != access.Testable.ID)
                    return (null, "У этой учётной записи нет доступа к указанному тестируемому.");
            }
            else
            {
                var filial = await _db.UserFilials.Include(x => x.Filial)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (filial?.Filial == null)
                    return (null, "Учётной записи не назначен филиал.");
                if (filial.Filial.ID != access.Testable.filial.ID)
                    return (null, "Тестируемый не относится к вашему филиалу.");
            }

            // Последний назначенный период может оказаться закрытым:
            // предыдущий активный период тогда не подходит.
            var latest = await _db.AccessForTestables
                .Where(x => x.Testable.ID == access.Testable.ID)
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, x.isActive })
                .FirstOrDefaultAsync();

            if (latest == null || latest.ID != access.ID)
                return (null, "Этот период больше не является последним.");
            if (!latest.isActive)
                return (null, "Последний период тестирования закрыт.");
            return (access, null);
        }

        private async Task<bool> IsCompleted(int accessId, int year)
        {
            var questions = await GetQuestions(year);
            if (questions.Length == 0) return false;
            var completed = await GetCompletedQuestionIds(accessId, year);
            return questions.All(x => completed.Contains(x.ID));
        }

        private Task<Comp_Question[]> GetQuestions(int year) =>
            _db.Comp_Questions.Include(x => x.Block)
                .Where(x => x.Block.Year == year)
                .OrderBy(x => x.QuestionNumber).ThenBy(x => x.ID).ToArrayAsync();

        private async Task<HashSet<int>> GetCompletedQuestionIds(int accessId, int year)
        {
            var saved = await _db.Comp_TestableAnswers
                .Where(x => x.Access.ID == accessId && x.Answer.Question.Block.Year == year)
                .Select(x => new { QuestionID = x.Answer.Question.ID, AnswerID = x.Answer.ID, x.Mark })
                .ToArrayAsync();

            return saved.GroupBy(x => x.QuestionID)
                .Where(g => g.Count() == 4 &&
                            g.Select(x => x.AnswerID).Distinct().Count() == 4 &&
                            g.Select(x => x.Mark).OrderBy(x => x)
                                .SequenceEqual(new[] { 1, 2, 3, 4 }))
                .Select(g => g.Key).ToHashSet();
        }

        [HttpGet]
        public async Task<IActionResult> Question(int accessId, int year)
        {
            if (!ValidYear(year)) return NotFound();
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            var (access, error) = await CheckAccess(accessId, user);
            if (access == null) return TestError(error!);

            var questions = await GetQuestions(year);
            if (questions.Length == 0)
                return TestError($"Вопросы для теста «Компетенции ({year} год)» ещё не загружены.");

            var completed = await GetCompletedQuestionIds(access.ID, year);
            var next = questions.FirstOrDefault(x => !completed.Contains(x.ID));
            if (next == null)
                return RedirectToAction(nameof(Finish), new { accessId, year });

            var answers = await _db.Comp_Answers.AsNoTracking()
                .Where(x => x.Question.ID == next.ID)
                .OrderBy(x => x.Number).ThenBy(x => x.ID)
                .Select(x => new MVC_CompetentionModel_Answer
                {
                    ID = x.ID,
                    AnswerNumber = x.Number,
                    AnswerText = x.Text
                }).ToArrayAsync();

            if (answers.Length != 4)
                return TestError($"Для ситуации №{next.QuestionNumber} должно быть ровно 4 варианта ответа.");

            return View(new MVC_CompetentionModel_Question
            {
                AccessID = access.ID,
                Year = year,
                QuestionID = next.ID,
                Number = next.QuestionNumber,
                Text = next.QuestionText,
                Position = Array.FindIndex(questions, x => x.ID == next.ID) + 1,
                TotalQuestions = questions.Length,
                Answers = answers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Question(MVC_CompetentionSubmitModel model)
        {
            if (!ValidYear(model.Year)) return NotFound();
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            var (access, error) = await CheckAccess(model.AccessID, user);
            if (access == null) return TestError(error!);

            await using var transaction =
                await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"AccessForTestables\" WHERE \"ID\" = {access.ID} FOR UPDATE");

            // Вторая проверка внутри транзакции защищает от закрытия периода
            // между открытием страницы и отправкой результата.
            var latest = await _db.AccessForTestables
                .Where(x => x.Testable.ID == access.Testable.ID)
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, x.isActive }).FirstOrDefaultAsync();
            if (latest == null || latest.ID != access.ID || !latest.isActive)
                return TestError("Период тестирования изменился или закрыт.");

            var questions = await GetQuestions(model.Year);
            if (questions.Length == 0)
                return TestError("Для выбранного года не загружены ситуации.");

            var completed = await GetCompletedQuestionIds(access.ID, model.Year);
            var next = questions.FirstOrDefault(x => !completed.Contains(x.ID));
            if (next == null)
                return RedirectToAction(nameof(Finish),
                    new { accessId = access.ID, year = model.Year });

            // Устаревшая вкладка или повторная отправка: не изменяем уже
            // сохранённую ситуацию и показываем актуальный вопрос.
            if (model.QuestionID != next.ID)
                return RedirectToAction(nameof(Question),
                    new { accessId = access.ID, year = model.Year });

            var variants = await _db.Comp_Answers.Include(x => x.Question)
                .Where(x => x.Question.ID == next.ID)
                .OrderBy(x => x.Number).ThenBy(x => x.ID).ToArrayAsync();
            if (variants.Length != 4)
                return TestError("Для текущей ситуации должно быть ровно четыре варианта действий.");

            var formAnswers = model.Answers ?? Array.Empty<MVC_CompetentionModel_Answer>();
            bool valid = formAnswers.Length == 4
                && formAnswers.All(x => x.Priority.HasValue && x.Priority.Value is >= 1 and <= 4)
                && formAnswers.Select(x => x.Priority!.Value).OrderBy(x => x)
                    .SequenceEqual(new[] { 1, 2, 3, 4 })
                && formAnswers.Select(x => x.ID).OrderBy(x => x)
                    .SequenceEqual(variants.Select(x => x.ID).OrderBy(x => x));

            if (!valid)
            {
                ModelState.AddModelError("",
                    "Каждому из четырёх вариантов необходимо назначить уникальный приоритет от 1 до 4.");
                return View(new MVC_CompetentionModel_Question
                {
                    AccessID = access.ID,
                    Year = model.Year,
                    QuestionID = next.ID,
                    Number = next.QuestionNumber,
                    Position = Array.FindIndex(questions, x => x.ID == next.ID) + 1,
                    TotalQuestions = questions.Length,
                    Text = next.QuestionText,
                    Answers = variants.Select(x => new MVC_CompetentionModel_Answer
                    {
                        ID = x.ID,
                        AnswerNumber = x.Number,
                        AnswerText = x.Text,
                        Priority = formAnswers.FirstOrDefault(y => y.ID == x.ID)?.Priority
                    }).ToArray()
                });
            }

            // При восстановлении после частично сохранённых старых данных
            // удаляем неполный набор приоритетов и заменяем его целиком.
            var old = await _db.Comp_TestableAnswers
                .Where(x => x.Access.ID == access.ID && x.Answer.Question.ID == next.ID)
                .ToArrayAsync();
            _db.Comp_TestableAnswers.RemoveRange(old);

            var ranks = formAnswers.ToDictionary(x => x.ID, x => x.Priority!.Value);
            var timestamp = DateTime.UtcNow;
            var newAnswers = variants.Select(x => new Comp_TestableAnswer
            {
                Access = access,
                Answer = x,
                Mark = ranks[x.ID],
                DateTimeAnswer = timestamp
            }).ToArray();
            _db.Comp_TestableAnswers.AddRange(newAnswers);
            await _db.SaveChangesAsync();

            var stored = await _db.TestResults.Include(x => x.Access)
                .FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            var aggregate = stored == null
                ? new JSON_ResultModel { AccessID = access.ID, TestableCode = access.Testable.Code }
                : JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)
                    ?? throw new InvalidOperationException("Не удалось прочитать общий JSON результатов.");

            aggregate.SaveCompetentionQuestion(model.Year, next, newAnswers);

            // Итоговый расчёт выполняется один раз после последней
            // ситуации этого года. Другой год и остальные тесты не трогаем.
            bool finished = questions.All(x => x.ID == next.ID || completed.Contains(x.ID));
            if (finished)
            {
                var allAnswers = await _db.Comp_TestableAnswers
                    .Include(x => x.Answer).ThenInclude(x => x.Question)
                    .ThenInclude(x => x.Block)
                    .Where(x => x.Access.ID == access.ID && x.Answer.Question.Block.Year == model.Year)
                    .ToArrayAsync();

                var blocks = await _db.Comp_Blocks.Where(x => x.Year == model.Year)
                    .ToArrayAsync();
                aggregate.CalculateCompetentionYear(model.Year, blocks, allAnswers);
            }

            if (stored == null)
                _db.TestResults.Add(new TestResult
                {
                    Access = access,
                    Results = aggregate.GetJSON()
                });
            else
                stored.Results = aggregate.GetJSON();

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            if (finished)
                return RedirectToAction(nameof(Finish),
                    new { accessId = access.ID, year = model.Year });
            return RedirectToAction(nameof(Question),
                new { accessId = access.ID, year = model.Year });
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int accessId, int year)
        {
            if (!ValidYear(year)) return NotFound();
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            var (access, error) = await CheckAccess(accessId, user);
            if (access == null) return TestError(error!);
            if (!await IsCompleted(access.ID, year))
                return RedirectToAction(nameof(Question), new { accessId, year });

            var stored = await _db.TestResults
                .Where(x => x.Access.ID == access.ID)
                .Select(x => x.Results).FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(stored))
                return TestError("Ответы сохранены, но итоговые результаты не рассчитаны.");

            var result = JsonSerializer.Deserialize<JSON_ResultModel>(stored);
            var blockIds = await _db.Comp_Blocks
                .Where(x => x.Year == year).Select(x => x.ID).ToArrayAsync();
            if (blockIds.Length == 0 ||
                result?.CompetentionResult == null ||
                !blockIds.All(id => result.CompetentionResult
                    .Any(x => x.Year == year && x.BlockID == id)))
                return TestError("Ответы сохранены, но итоговый расчёт компетенций отсутствует.");

            ViewBag.Year = year;
            return View();
        }

        private IActionResult TestError(string message)
        {
            ViewBag.Error = message;
            return View("CompetentionError");
        }
    }
}
