namespace ManagmentSystem.Presentation.Areas.Admin.Models.UserVMs
{
    public class AdminUserListVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public bool LockoutEnabled { get; set; }
       
    }
}
