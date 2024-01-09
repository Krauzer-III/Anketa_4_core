using Anketa_4_core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_MotivationModel : IValidatableObject
    {
        [HiddenInput]
        public int AccessTestID { get; set; }
        public bool IsAgree { get; set; }
        public string RecomendPosition { get; set; }
        public string Recomendator { get; set; }
        public string Question_01 { get; set; }
        public string Question_02 { get; set; }
        public string Question_03 { get; set; }
        public string Question_04 { get; set; }
        public string Question_05 { get; set; }
        public string Question_06 { get; set; }
        public string Question_07 { get; set; }
        public string Question_08 { get; set; }
        public string Question_09 { get; set; }
        public string Question_YES_NO { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class MVC_MotivationLoginModel : IValidatableObject
    {
        public string Code { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            var result = ValidateHelper.MotivationTestReadyTesting(Code);
            if (!result.result)
                errors.Add(new ValidationResult(result.error));

            return errors;
        }
    }
}
