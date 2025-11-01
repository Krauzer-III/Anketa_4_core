using System;
using System.Linq;
using System.Threading.Tasks;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Moderator
{
    [Authorize(Roles = "admin,moderator")]
    public class NewsAdminController : Controller
    {
        private readonly AnketaContext _db;
        public NewsAdminController(AnketaContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _db.News.AsNoTracking()
                .OrderByDescending(n => n.DateNews).ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var n = new News { DateNews = DateTime.UtcNow };
            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News model)
        {
            if (string.IsNullOrWhiteSpace(model.Header))
                ModelState.AddModelError(nameof(model.Header), "Заголовок обязателен");
            if (string.IsNullOrWhiteSpace(model.Body))
                ModelState.AddModelError(nameof(model.Body), "Текст обязателен");

            if (!ModelState.IsValid) return View(model);

            model.DateNews = DateTime.UtcNow; // фиксируем текущую дату публикации
            _db.News.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var n = await _db.News.FirstOrDefaultAsync(x => x.ID == id);
            if (n == null) return NotFound();
            n.DateNews = DateTime.UtcNow;
            return View(n);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News model)
        {
            if (id != model.ID) return BadRequest();
            if (string.IsNullOrWhiteSpace(model.Header))
                ModelState.AddModelError(nameof(model.Header), "Заголовок обязателен");
            if (string.IsNullOrWhiteSpace(model.Body))
                ModelState.AddModelError(nameof(model.Body), "Текст обязателен");
            if (!ModelState.IsValid) return View(model);

            var n = await _db.News.FirstOrDefaultAsync(x => x.ID == id);
            if (n == null) return NotFound();

            n.Header = model.Header;
            n.Body = model.Body;
            n.DateNews = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var n = await _db.News.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (n == null) return NotFound();
            return View(n);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var n = await _db.News.FirstOrDefaultAsync(x => x.ID == id);
            if (n == null) return NotFound();
            _db.News.Remove(n);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
