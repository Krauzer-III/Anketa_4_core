using Anketa_4_core.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Helpers
{
    public static class ValidateHelper
    {
        private static Dictionary<string, string> errors = new Dictionary<string, string>() {
            { "CodeExist", "Такого кода тестируемого не существует." },
            { "AccessExist", "Тестируемому не назначен перирод оценки." },
            { "MotivationTestReadyTesting", "Этот тестируемый уже проходил мотивационный опросник. Или ему не назначен новый период тестирования." },
            { "360TestReadyTesting", "Этот тестируемый уже проходил самооценку 360 градусов. Или ему не назначен новый период тестирования." },
            { "CompetentionTestReadyTesting", "Этот тестируемый уже проходил тест компетенций. Или ему не назначен новый период тестирования." },
            { "KettelTestReadyTesting", "Этот тестируемый уже проходил психологическое тестирование. Или ему не назначен новый период тестирования." }
        };
        public static bool CodeExist(string code)
        {
            using (var context = new AnketaContext())
            {
                return context.Testables.Any(t => t.Code == code);
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
                    return (false, errors["CodeExist"]);
                var lastAccess = context.AccessForTestables.Include(t => t.Testable).LastOrDefault(x => x.Testable.Code == code);
                if (lastAccess != null)
                    return (false, errors["AccessExist"]);
                if (context.MotivationTests.Include(x => x.AccessTest).Any(t => t.AccessTest.ID == lastAccess.ID))
                    return (false, errors["MotivationTestReadyTesting"]);
                return (true, "");
            }
        }
    }
}
