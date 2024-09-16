using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.UserVMs
{
    public class AdminUserEditVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Mail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100, ErrorMessage = "Address can't be longer than 100 characters.")]
        public string Address { get; set; }

        public string? ProfileImage { get; set; } // Optional field, no annotations
        public IFormFile? ProfileImagePath { get; set; }






    }
}
