using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models
{
    public class MVC_LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Модель для передачи данных при входе в систему (логин и пароль).
    /// Регистрация и передача данных для аутентификации пользователя.
    /// </summary>
    public class LoginDataModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Регистрация и передача данных для аутентификации пользователя c привязкой к филиалу.
    /// </summary>
    public class LoginFilialDataModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FilialName { get; set; }
    }

    /// <summary>
    /// Регистрация и передача данных для аутентификации пользователя c привязкой к коду.
    /// </summary>
    public class LoginDataWithCodeModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Code { get; set; } //существующий код тестируемого
    }

    /// <summary>
    /// Регистрация и передача данных для аутентификации пользователя c созданием профиля тестируемого.
    /// </summary>
    public class LoginDataWithTestableModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string FilialName { get; set; }
        public int ReservLevelID { get; set; }
        public int TestablePeriodID { get; set; }
    }

    public static class RegistrationRoles
    {
        public const string Admin = "admin";
        public const string Moderator = "moderator";
        public const string Verificator = "verificator";
        public const string TestableUser = "testable_user";
        public const string FilialUser = "filial_user";
    }

    public static class TestableRegistrationModes
    {
        public const string Existing = "existing";
        public const string New = "new";
    }


    public class RegistrationPageModel
    {
        [Required(ErrorMessage = "Выберите роль пользователя")]
        public string Role { get; set; } = RegistrationRoles.Admin;

        public string TestableMode { get; set; } = TestableRegistrationModes.Existing;

        [Required(ErrorMessage = "Укажите разделитель")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Разделитель должен быть одним символом")]
        public string Delimiter { get; set; } = ";";

        public string? TextData { get; set; }

        public IFormFile? CsvFile { get; set; }

        public List<RegistrationErrorModel> Errors { get; set; } = new();

        public int ParsedCount { get; set; }
        public int CreatedCount { get; set; }

        public bool IsSuccess => CreatedCount > 0 && Errors.Count == 0;
    }

    public class RegistrationErrorModel
    {
        public int Line { get; set; }
        public string? Login { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
