using Anketa_4_core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Anketa_4_core.Controllers
{
    [Authorize(Roles = "testable_user")]
    public class PersonalCabinetController : Controller
    {
        private readonly AnketaContext _anketaContext;
        private readonly ILogger<PersonalCabinetController> _logger;

        public PersonalCabinetController(AnketaContext anketaContext, ILogger<PersonalCabinetController> logger)
        {
            _anketaContext = anketaContext;
            _logger = logger;
        }

        // Главная страница личного кабинета
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }

            var testableUser = await _anketaContext.TestableUsers
                .Include(tu => tu.testable)
                .ThenInclude(t => t.filial)
                .FirstOrDefaultAsync(tu => tu.UserName == userName);

            if (testableUser == null)
            {
                _logger.LogWarning("Не найден TestableUser для пользователя {UserName}", userName);
                return NotFound("Данные пользователя не найдены.");
            }

            // Парсим JSON-поле LK_Data (если есть)
            dynamic? lkData = null;
            if (!string.IsNullOrEmpty(testableUser.LK_Data))
            {
                try
                {
                    lkData = JsonConvert.DeserializeObject(testableUser.LK_Data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка парсинга LK_Data для пользователя {UserName}", userName);
                }
            }

            // Пример модели для представления
            var model = new
            {
                Filial = testableUser.testable?.filial?.FilialName,
                Code = testableUser.testable?.Code,
                LK_Data = lkData
            };

            return Json(model);
        }

        // Сохранение данных личного кабинета (в JSON)
        [HttpPost]
        public async Task<IActionResult> SaveData([FromBody] object lkData)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }

            var testableUser = await _anketaContext.TestableUsers.FirstOrDefaultAsync(tu => tu.UserName == userName);
            if (testableUser == null)
            {
                return NotFound("Пользователь не найден.");
            }

            testableUser.LK_Data = lkData.ToString();
            await _anketaContext.SaveChangesAsync();

            return Ok(new { message = "Данные успешно сохранены." });
        }
    }
}
