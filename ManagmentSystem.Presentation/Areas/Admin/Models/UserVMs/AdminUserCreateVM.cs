namespace ManagmentSystem.Presentation.Areas.Admin.Models.UserVMs
{
    public class AdminUserCreateVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string? ProfileImage { get; set; }
    }
}
