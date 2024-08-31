using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductUpdateVM
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public List<CategorySelectModel>? Categories { get; set; }
        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();
    }
}
