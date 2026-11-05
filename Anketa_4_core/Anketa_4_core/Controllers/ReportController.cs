using Anketa_4_core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    /// <summary>
    /// Единая точка получения отчёта из личного кабинета и Testable/Details.
    /// TODO: подключить генератор ReportGenerator и шаблон документа.
    /// </summary>
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AnketaContext _db;
        public ReportController(AnketaContext db) => _db = db;

        /// <summary>
        /// Множественный выбор из модуля результатов Anketa 3.
        /// Это та же точка действия GetReport, что и в личном кабинете,
        /// но пока только проверяет набор периодов и показывает заготовку.
        /// TODO: подключить ReportGenerator к массиву уже рассчитанных JSON.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator,verificator")]
        public async Task<IActionResult> GetReport([FromForm] int[] accessIds)
        {
            if (accessIds == null) return BadRequest("Не выбраны периоды.");
            var ids = accessIds.Where(x => x > 0).Distinct().ToArray();
            if (ids.Length == 0 || ids.Length > 100)
                return BadRequest("Выберите от 1 до 100 назначений периода.");

            var records = await _db.AccessForTestables.AsNoTracking()
                .Where(x => ids.Contains(x.ID))
                .Select(x => new
                {
                    x.ID, Code = x.Testable.Code,
                    Period = x.TestPeriod.GroupName
                }).OrderBy(x => x.Code).ToListAsync();

            if (records.Count != ids.Length)
                return BadRequest("Среди выбранных записей есть несуществующие периоды.");

            ViewBag.BatchCount = records.Count;
            ViewBag.BatchList = records.Select(x =>
                x.Code + " — " + x.Period + " (доступ №" + x.ID + ")").ToArray();
            return View("Pending");
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(int? testableId)
        {
            bool moderator = User.IsInRole("admin") || User.IsInRole("moderator");
            int id;
            if (moderator && testableId.HasValue)
                id = testableId.Value;
            else
            {
                if (!User.IsInRole("testable_user")) return Forbid();

                string? username = User.Identity?.Name;
                var ownId = await _db.TestableUsers.AsNoTracking()
                    .Where(x => x.UserName == username)
                    .Select(x => (int?)x.testable.ID).FirstOrDefaultAsync();
                if (!ownId.HasValue) return NotFound();
                if (testableId.HasValue && testableId != ownId.Value) return Forbid();
                id = ownId.Value;
            }

            var testable = await _db.Testables.AsNoTracking()
                .Where(x => x.ID == id)
                .Select(x => new { x.ID, x.Code }).FirstOrDefaultAsync();
            if (testable == null) return NotFound();

            // TODO: GetReport will use the last assigned AccessForTestable
            // and load TestResult.Results for ReportGenerator.
            var latest = await _db.AccessForTestables.AsNoTracking()
                .Where(x => x.Testable.ID == testable.ID)
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, Period = x.TestPeriod.GroupName })
                .FirstOrDefaultAsync();

            ViewBag.TestableID = testable.ID;
            ViewBag.TestableCode = testable.Code;
            ViewBag.AccessID = latest?.ID;
            ViewBag.PeriodName = latest?.Period;
            return View("Pending");
        }
    }
}
