namespace ManagmentSystem.Presentation.Areas.Admin.Models.UserVMs
{
    public class AdminUserEditVM
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
