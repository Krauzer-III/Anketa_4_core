using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_Test360Model
    {
        public int AccesTestID { get; set; }
        public int RespondentRoleID { get; set; }
        public bool isSelf { get; set; }
        public int q01 { get; set; }
        public int q02 { get; set; }
        public int q03 { get; set; }
        public int q04 { get; set; }
        public int q05 { get; set; }
        public int q06 { get; set; }
        public int q07 { get; set; }
        public int q08 { get; set; }
        public int q09 { get; set; }
        public int q10 { get; set; }
        public int q11 { get; set; }
        public int q12 { get; set; }
        public int q13 { get; set; }
        public int q14 { get; set; }
        public int q15 { get; set; }
    }

    public class MVC_Test360_StartTest_Model : IValidatableObject
    {
        [Required]
        [Display(Name = "Код оцениваемого")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Роль респондента")]
        public int RoleID { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            using (var context = new AnketaContext())
            {
                //Проверяем есть ли вообще такой код
                Testable? t = context.Testables.Include(f => f.filial).FirstOrDefault(t => t.Code == Code);
                if (t == null)
                {
                    errors.Add(new ValidationResult("Такого кода не существует в базе"));
                }
                else
                {
                    //Проверяем филиал юзера
                    //TODO Сделать нормального вторизованного юзера
                    string UserName = "";
                    UserFilial? fil = context.UserFilials.Include(f => f.Filial).FirstOrDefault(u => u.UserName == UserName);
                    if (fil == null)
                    {
                        errors.Add(new ValidationResult("Пользователю не назначен филиал, обратитесь к администратору системы"));
                    }
                    else
                    {
                        //Проверяем, принадлежит ли юзер филиалу введённого кода
                        if (fil.Filial.ID != t.filial.ID)
                        {
                            errors.Add(new ValidationResult("Пользователю не принадлежит филиалу введённого кода"));
                        }
                        else
                        {
                            var accesForTestableID = context.AccessForTestables.Include(t => t.Testable).LastOrDefault(t => t.Testable.Code == Code);
                            if (accesForTestableID == null)
                            {
                                errors.Add(new ValidationResult("Пользователю не назначено тестирование"));
                            }
                            else
                            {
                                if (context.Test360.Include(a => a.AccessTest).Include(r => r.Role).FirstOrDefault(t => t.AccessTest.ID == accesForTestableID.ID && t.Role.isSelfMark) != null)
                                {
                                    errors.Add(new ValidationResult("Пользователю уже проходил самооценку"));
                                }
                                if (accesForTestableID.isActive)
                                {
                                    errors.Add(new ValidationResult("Прохождение тестирования закрыто"));
                                }
                            }
                        }
                    }
                }
            }
            return errors;
        }
    }
}
