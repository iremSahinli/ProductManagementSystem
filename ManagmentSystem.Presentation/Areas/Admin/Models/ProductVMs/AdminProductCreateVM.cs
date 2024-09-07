using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductCreateVM
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();
        public List<CategorySelectModel>? Categories { get; set; }
        public Guid SelectedCategoryId { get; set; } // Tekil seçim için

    }
}
