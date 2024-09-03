using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }    
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public List<string> CategoryName { get; set; } = new List<string>();
        
    }
}
