using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.ProductVMs
{
    public class AdminProductUpdateVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0.")]
        public double ProductPrice { get; set; }
        [Required(ErrorMessage = "You must write a description.")]
        [StringLength(128, ErrorMessage = "Product description can't be longer than 128 characters.")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "You must select a category.")]
        public List<CategorySelectModel>? Categories { get; set; }
        [Required(ErrorMessage = "You must select at least one category.")]
        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();
    }
}
