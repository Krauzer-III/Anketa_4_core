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

    public class MVC_Test360_StartTest_Model
    {
        [Required]
        [Display(Name = "Код оцениваемого")]
        public string Code { get; set; } = "";

        [Required]
        [Display(Name = "Роль респондента")]
        public int RoleID { get; set; }
    }
}
