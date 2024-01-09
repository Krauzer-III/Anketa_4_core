using Anketa_4_core.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_TestableAdd : IValidatableObject
    {
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            using(var context = new AnketaContext())
            {
                if (Code == "" || Code == null)
                    errors.Add(new ValidationResult("Код не должен быть пустым"));
                if (context.Testables.Any(x=>x.Code==Code))
                    errors.Add(new ValidationResult($"Тестируемый с таким номером уже существует в базе"));
                if (context.Filials.FirstOrDefault(f => f.ID == Filial) == null)
                    errors.Add(new ValidationResult("Некорректно указан филиал"));
                if (context.ReservLevels.FirstOrDefault(f => f.ID == RezervLevel) == null)
                    errors.Add(new ValidationResult("Некорректно указан уровень резерва"));
            }

            return errors;
        }
    }

    public class MVC_TestableEdit : IValidatableObject
    {
        [HiddenInput]
        public int ID { get; set; }
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            using (var context = new AnketaContext())
            {
                var oldcode = context.Testables.First(f => f.ID == ID).Code;

                if (Code == "" || Code == null)
                    errors.Add(new ValidationResult("Код не должен быть пустым"));
                if (context.Testables.Any(x => x.Code == Code && x.Code!=oldcode))
                    errors.Add(new ValidationResult($"Тестируемый с таким номером уже существует в базе"));
                if (context.Filials.FirstOrDefault(f => f.ID == Filial) == null)
                    errors.Add(new ValidationResult("Некорректно указан филиал"));
                if (context.ReservLevels.FirstOrDefault(f => f.ID == RezervLevel) == null)
                    errors.Add(new ValidationResult("Некорректно указан уровень резерва"));
            }

            return errors;
        }
    }

    public class MVC_TestableDetails
    {
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }
        //TODO MVC представления тестов и периодов


    }

    public class MVC_TestableView
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public string FilialName { get; set; }
        [Display(Name = "Уровень резерва")]
        public string ReservLevelName { get; set; }
        [Display(Name = "Год резерва")]
        public int YearTraining { get; set; }
        [Display(Name = "В архиве")]
        public bool isArchived { get; set; }
    }
}
