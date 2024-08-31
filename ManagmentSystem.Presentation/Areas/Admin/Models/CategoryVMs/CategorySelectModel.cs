namespace ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs
{
    public class CategorySelectModel
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public bool IsSelected { get; set; }
    }
}
