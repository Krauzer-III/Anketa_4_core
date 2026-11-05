using Anketa_4_core.Data;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Anketa_4_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    /// <summary>
    /// Просмотр процедуры оценки. Портирован с Anketa 3:
    /// фильтры, статусы, детализация в модальных окнах.
    /// Все результаты получаем из TestResult.Results (JSON_ResultModel).
    /// </summary>
    [Authorize(Roles = "admin,moderator,verificator")]
    public class ResultController : Controller
    {
        private readonly AnketaContext _db;
        private readonly ResultModuleService _results;

        public ResultController(AnketaContext db)
        {
            _db = db;
            _results = new ResultModuleService(db);
        }

        [HttpGet]
        public async Task<IActionResult> IndexModerator()
        {
            var periods = await _db.TestPeriods.AsNoTracking()
                .OrderByDescending(x => x.ID)
                .Select(x => new { x.ID, x.GroupName }).ToListAsync();
            var filials = await _db.Filials.AsNoTracking()
                .OrderBy(x => x.FilialName)
                .Select(x => new { x.ID, x.FilialName }).ToListAsync();
            var reserves = await _db.ReservLevels.AsNoTracking()
                .OrderBy(x => x.ReservLevelName)
                .Select(x => new { x.ID, x.ReservLevelName }).ToListAsync();

            return View(new MVC_ResultIndex
            {
                Periods = periods.Select(x => new SelectListItem(x.GroupName, x.ID.ToString())).ToList(),
                Filials = filials.Select(x => new SelectListItem(x.FilialName, x.ID.ToString())).ToList(),
                ReservLevels = reserves.Select(x => new SelectListItem(x.ReservLevelName, x.ID.ToString())).ToList()
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchResult([FromQuery] MVC_ResultFilter filter)
        {
            if (!ModelState.IsValid) return BadRequest("Некорректные фильтры.");
            try
            {
                return PartialView("_SearchResult", await _results.SearchAsync(filter));
            }
            catch (Exception ex) when (ex is InvalidOperationException
                or System.Text.Json.JsonException)
            {
                Response.StatusCode = 500;
                return Content("Не удалось загрузить результаты: " + ex.Message);
            }
        }

        [HttpGet]
        public Task<IActionResult> GetTests360(int accessId) =>
            ShowAsync(accessId, "GetTests360");

        [HttpGet]
        public Task<IActionResult> GetMotivationTest(int accessId) =>
            ShowAsync(accessId, "GetMotivationTest");

        [HttpGet]
        public Task<IActionResult> GetKettelTest(int accessId) =>
            ShowAsync(accessId, "GetKettelTest");

        [HttpGet]
        public Task<IActionResult> GetCompetentionTest(int accessId, int year = 1) =>
            ShowAsync(accessId, "GetCompetentionTest", year);

        private async Task<IActionResult> ShowAsync(int accessId, string partial, int year = 0)
        {
            if (accessId <= 0 || (partial == "GetCompetentionTest" && year is not (1 or 2)))
                return BadRequest("Неверные параметры.");
            try
            {
                var detail = await _results.DetailAsync(accessId);
                if (detail == null) return NotFound("Назначение периода не найдено.");
                detail.Year = year;
                return PartialView(partial, detail);
            }
            catch (InvalidOperationException ex)
            {
                return PartialView("_ResultLoadError", ex.Message);
            }
        }
    }
}
