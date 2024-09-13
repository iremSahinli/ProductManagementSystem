using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Models.AccountVM
{
    public class ForgotPasswordVM
    {

        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
