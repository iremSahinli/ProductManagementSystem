using ManagmentSystem.Domain.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public Category()
        {
            ProductCategories = new HashSet<ProductCategory>();
            SubCategories = new HashSet<Category>(); 
        }

        [Required(ErrorMessage = "The CategoryName field is required.")]
        [MaxLength(128, ErrorMessage = "The CategoryName must be less than 128 characters.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [MaxLength(128, ErrorMessage = "The Description must be less than 128 characters.")]
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }  
        public virtual Category ParentCategory { get; set; }  

        public virtual ICollection<Category> SubCategories { get; set; } 

        // Nav Prop
        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }
    }

}
