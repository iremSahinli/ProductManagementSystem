using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs
{
    public class AdminCategoryUpdateVM
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name can't be longer than 100 characters.")]
        public string CategoryName { get; set; }

        [StringLength(250, ErrorMessage = "Description can't be longer than 250 characters.")]
        public string Description { get; set; }
    }
}
