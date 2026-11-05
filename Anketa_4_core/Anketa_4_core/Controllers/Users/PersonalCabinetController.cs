using Anketa_4_core.Data;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Anketa_4_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    [Authorize(Roles = "testable_user")]
    public class PersonalCabinetController : Controller
    {
        private readonly AnketaContext _db;
        private readonly ILogger<PersonalCabinetController> _logger;
        public PersonalCabinetController(
            AnketaContext db, ILogger<PersonalCabinetController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() => View();

        /// <summary>
        /// Receives all section values for the AJAX dashboard, without exposing
        /// another testable's data. The HTML view is returned by Index().
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Data()
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName)) return Unauthorized();
            try
            {
                var lk = await new LKCabinetService(_db).LoadForUserAsync(userName);
                return Json(new { lkData = lk });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSection([FromBody] MVC_LK_SectionUpdate request)
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName)) return Unauthorized();
            if (request == null) return BadRequest(new { error = "Данные не получены." });
            try
            {
                var lk = await new LKCabinetService(_db)
                    .SaveForUserAsync(userName, request.Section, request.Value);
                return Json(new { saved = true, lkData = lk });
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException
                or System.Text.Json.JsonException or NotSupportedException)
            {
                _logger.LogWarning(ex, "LK section validation failed for {User}", userName);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Rating()
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName)) return Unauthorized();

            var user = await _db.TestableUsers
                .Where(x => x.UserName == userName)
                .Select(x => new { TestableID = x.testable.ID }).FirstOrDefaultAsync();
            if (user == null) return NotFound(new { error = "Не найден тестируемый." });

            var snapshot = await new LKPeriodRatingService(_db)
                .GetForTestableAsync(user.TestableID);
            return Json(snapshot);
        }
    }
}
