using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Models.AccountVM
{
    public class UserProfileVM
    {
        [Required(ErrorMessage = "First name is required.")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]

        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address can't be longer than 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]

        public string Mail { get; set; }

        public IFormFile? ProfileImage { get; set; }

    }
}
