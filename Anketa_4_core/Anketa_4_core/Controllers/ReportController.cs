using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Anketa_4_core.Controllers
{
    /// <summary>
    /// One Excel report endpoint for the personal cabinet, Testable/Details
    /// and multiple selected rows from the moderator results module.
    /// Reads only the already computed JSON from TestResult.Results.
    /// </summary>
    [Authorize]
    public class ReportController : Controller
    {
        private const int MaxParticipants = 100;
        private readonly AnketaContext _db;
        private readonly IWebHostEnvironment _environment;

        public ReportController(AnketaContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(int? testableId)
        {
            bool moderator = User.IsInRole("admin") || User.IsInRole("moderator");
            int id;
            if (moderator && testableId.HasValue)
            {
                id = testableId.Value;
            }
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

            var accessId = await _db.AccessForTestables.AsNoTracking()
                .Where(x => x.Testable.ID == id)
                .OrderByDescending(x => x.ID)
                .Select(x => (int?)x.ID).FirstOrDefaultAsync();

            if (!accessId.HasValue)
                return BadRequest("У тестируемого нет назначенных периодов оценки.");

            return await DownloadAsync(new[] { accessId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator,verificator")]
        public async Task<IActionResult> GetReport([FromForm] int[] accessIds)
        {
            if (accessIds == null)
                return BadRequest("Не выбраны периоды.");

            var selected = accessIds.Where(x => x > 0).Distinct().ToArray();
            if (selected.Length < 1 || selected.Length > MaxParticipants)
                return BadRequest($"Выберите от 1 до {MaxParticipants} назначений периода.");

            return await DownloadAsync(selected);
        }

        private async Task<IActionResult> DownloadAsync(int[] accessIds)
        {
            var assignments = await _db.AccessForTestables.AsNoTracking()
                .Include(x => x.Testable).ThenInclude(x => x.filial)
                .Include(x => x.TestPeriod)
                .Where(x => accessIds.Contains(x.ID))
                .ToArrayAsync();

            if (assignments.Length != accessIds.Length)
                return BadRequest("Один или несколько периодов тестирования не найдены.");
            if (assignments.Select(x => x.TestPeriod.ID).Distinct().Count() != 1)
                return BadRequest("Для общего отчёта необходимо выбрать один период тестирования.");

            var saved = await _db.TestResults.AsNoTracking()
                .Include(x => x.Access)
                .Where(x => accessIds.Contains(x.Access.ID))
                .ToArrayAsync();

            var duplicate = saved.GroupBy(x => x.Access.ID)
                .FirstOrDefault(x => x.Count() > 1);
            if (duplicate != null)
                return Conflict($"Для назначения {duplicate.Key} найдено несколько TestResult.");

            var byAccess = saved.ToDictionary(x => x.Access.ID, x => x.Results);
            var subjects = new List<ReportGenerator.Subject>();
            foreach (var access in assignments.OrderBy(x => x.Testable.Code))
            {
                var data = new JSON_ResultModel
                {
                    AccessID = access.ID,
                    TestableCode = access.Testable.Code
                };

                if (byAccess.TryGetValue(access.ID, out var raw))
                {
                    try
                    {
                        data = JsonSerializer.Deserialize<JSON_ResultModel>(raw)
                            ?? throw new JsonException("JSON не содержит данных.");
                    }
                    catch (Exception ex) when (ex is JsonException or NotSupportedException)
                    {
                        return Conflict($"Некорректный JSON для кода {access.Testable.Code}: {ex.Message}");
                    }

                    if (data.AccessID != 0 && data.AccessID != access.ID)
                        return Conflict($"JSON тестируемого {access.Testable.Code} относится к другому периоду.");
                }

                subjects.Add(new ReportGenerator.Subject
                {
                    AccessID = access.ID,
                    Code = access.Testable.Code,
                    Filial = access.Testable.filial?.FilialName ?? "—",
                    // Old project received these from an external personnel directory;
                    // Testable does not currently contain name/position.
                    Name = "—",
                    Position = "—",
                    Period = access.TestPeriod.GroupName,
                    Data = data
                });
            }

            string templatePath = Path.Combine(
                _environment.ContentRootPath, "Reports", "report.xlsx");

            try
            {
                byte[] bytes = ReportGenerator.Generate(subjects, templatePath);
                string filename = $"Отчёт_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
                return File(bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    filename);
            }
            catch (FileNotFoundException ex)
            {
                // Only the binary template must be deployed; code is ready.
                return StatusCode(503,
                    "Excel-шаблон пока не размещён в Reports/report.xlsx: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict("Не удалось сформировать Excel-отчёт: " + ex.Message);
            }
        }
    }
}
