using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
