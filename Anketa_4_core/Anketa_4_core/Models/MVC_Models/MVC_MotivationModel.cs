using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    /// <summary>
    /// Данные формы мотивационного опросника. Формат сохраняемого JSON
    /// остаётся Motivation_Result: вопрос 6 собирается из двух ответов,
    /// а варианты вопроса 8 объединяются в одну строку.
    /// </summary>
    public class MVC_MotivationModel
    {
        [HiddenInput]
        public int AccessTestID { get; set; }

        [Required(ErrorMessage = "Укажите, хотите ли Вы вступить в кадровый резерв.")]
        public bool? IsAgree { get; set; }

        [Required(ErrorMessage = "Укажите рекомендуемую должность резерва.")]
        public string? RecomendPosition { get; set; }

        [Required(ErrorMessage = "Укажите, по чьей рекомендации Вы включены в состав кандидатов.")]
        public string? Recomendator { get; set; }

        public string? Question_01 { get; set; }
        public string? Question_02 { get; set; }
        public string? Question_03 { get; set; }
        public string? Question_04 { get; set; }
        public string? Question_05 { get; set; }

        // В старом интерфейсе вопрос 6 состоит из двух полей.
        public string? Question_06_1 { get; set; }
        public string? Question_06_2 { get; set; }

        public string? Question_07 { get; set; }

        // Несколько выбранных регионов; в Motivation_Result.Question_08 — строка.
        public string[] Question_08 { get; set; } = Array.Empty<string>();

        public string? Question_09 { get; set; }

        // При согласии записываем "Да", иначе — причину отказа.
        public string? Question_YES_NO { get; set; }
    }

    public class MVC_MotivationLoginModel
    {
        [Required(ErrorMessage = "Введите код тестируемого.")]
        public string Code { get; set; } = "";
    }
}
