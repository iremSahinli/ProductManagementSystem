namespace ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs
{
    public class AdminCategoryListVM
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
