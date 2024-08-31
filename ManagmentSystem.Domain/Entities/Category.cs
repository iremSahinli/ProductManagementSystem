﻿using ManagmentSystem.Domain.Core.BaseEntities;
using System;
using System.Collections.Generic;
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
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Nav Prop
        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }

    }
}
