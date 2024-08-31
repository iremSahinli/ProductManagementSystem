using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public List<Guid> SelectedCategories { get; set; }=new List<Guid>();
    }
}
