using Anketa_4_core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class DefaultValuesController : Controller
    {
        private readonly AnketaContext _db;

        public DefaultValuesController(AnketaContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDefaultValues()
        {
            // Справочники и ключи вносятся идемпотентной миграцией AnketaContext.
            // Старые Add...() не вызываем: их повторный вызов дублирует вопросы.
            await _db.Database.MigrateAsync();
            return RedirectToAction(nameof(allAdded));
        }

        [HttpGet]
        public IActionResult allAdded() => View();
    }
}
