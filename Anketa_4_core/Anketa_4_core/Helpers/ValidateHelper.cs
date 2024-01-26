using Anketa_4_core.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Anketa_4_core.Helpers.ValidateHelper;
using static Anketa_4_core.Helpers.ValidTestableCode;

namespace Anketa_4_core.Helpers
{
    public static class ValidateHelper
    {
        public enum ErrorCodes
        {
            CodeExist, CodeNotExist , AccessExist, MotivationTestReadyTesting, _360TestReadyTesting , CompetentionTestReadyTesting, KettelTestReadyTesting
        }

        public static Dictionary<ErrorCodes, string> errors = new Dictionary<ErrorCodes, string>() {
            { ErrorCodes.CodeExist, "Такой код тестируемого уже существует." },
            { ErrorCodes.CodeNotExist, "Такого кода тестируемого не существует." },
            { ErrorCodes.AccessExist, "Тестируемому не назначен перирод оценки." },
            { ErrorCodes.MotivationTestReadyTesting, "Этот тестируемый уже проходил мотивационный опросник. Или ему не назначен новый период тестирования." },
            { ErrorCodes._360TestReadyTesting, "Этот тестируемый уже проходил самооценку 360 градусов. Или ему не назначен новый период тестирования." },
            { ErrorCodes.CompetentionTestReadyTesting, "Этот тестируемый уже проходил тест компетенций. Или ему не назначен новый период тестирования." },
            { ErrorCodes.KettelTestReadyTesting, "Этот тестируемый уже проходил психологическое тестирование. Или ему не назначен новый период тестирования." }
        };
        public static bool CodeExist(string code)
        {
            using (var context = new AnketaContext())
            {
                return context.Testables.Any(t => t.Code == code);
            }
        }

        public static bool CodeExist(string code, string oldcode)
        {
            using (var context = new AnketaContext())
            {
                return context.Testables.Any(t => t.Code == code && t.Code != oldcode);
            }
        }

        public static bool AccessExist(string code)
        {
            using (var context = new AnketaContext())
            {
                return context.AccessForTestables.Include(x => x.Testable).Any(t => t.Testable.Code == code);
            }
        }

        public static (bool result, string error) MotivationTestReadyTesting(string code)
        {
            using (var context = new AnketaContext())
            {
                if (!CodeExist(code))
                    return (false, errors[ErrorCodes.CodeNotExist]);
                var lastAccess = context.AccessForTestables.Include(t => t.Testable).LastOrDefault(x => x.Testable.Code == code);
                if (lastAccess != null)
                    return (false, errors[ErrorCodes.AccessExist]);
                if (context.MotivationTests.Include(x => x.AccessTest).Any(t => t.AccessTest.ID == lastAccess.ID))
                    return (false, errors[ErrorCodes.MotivationTestReadyTesting]);
                return (true, "");
            }
        }
    }


    public class ValidTestableCode : ValidationAttribute
    {
        public enum OperationType { Add, Edit }

        public OperationType operationType { get; set; }
        public string oldCode { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            string code = Convert.ToString(value);
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            if (code != null)
            {
                switch (operationType)
                {
                    case OperationType.Add:
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                        return CodeExist(code) ?
                            ValidationResult.Success :
                            new ValidationResult(ValidateHelper.errors[ErrorCodes.CodeExist]);
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                    case OperationType.Edit:
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                        return CodeExist(code, oldCode) ?
                         ValidationResult.Success :
                         new ValidationResult(ValidateHelper.errors[ErrorCodes.CodeExist]);
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                    default: return new ValidationResult("Неизвестная ошибка");

                }
            }
            else return new ValidationResult("Неизвестная ошибка");
        }
    }
}
