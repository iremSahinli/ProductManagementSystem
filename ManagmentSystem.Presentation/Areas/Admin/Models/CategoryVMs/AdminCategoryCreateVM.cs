using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs
{
    public class AdminCategoryCreateVM
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name can't be longer than 100 characters.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Category Discription is required.")]
        [StringLength(128, ErrorMessage = "Description can't be longer than 128 characters.")]
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }
    }
}
