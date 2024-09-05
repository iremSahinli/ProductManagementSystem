using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Models.AccountVM
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}
