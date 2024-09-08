using ManagmentSystem.Domain.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Entities
{
    public class Product:AuditableEntity
    {
        public Product()
        {
            ProductCategories=new HashSet<ProductCategory>();
        }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }

        // Nav Prop
        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }

    }
}
