namespace Anketa_4_core.Models.MVC_Models
{
    public class MVC_Account
    {
        public int UserId { get; set; }
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
        public bool MakeNewPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
