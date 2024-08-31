using ManagmentSystem.Domain.Core.BaseEntityConfiguration;
using ManagmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Configurations
{
   public class ProductConfiguration:AuditableEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p =>p.ProductName).IsRequired().HasMaxLength(128);
            builder.Property(p => p.ProductDescription).IsRequired().HasMaxLength(128);
            builder.Property(p => p.ProductPrice).IsRequired();
            base.Configure(builder);
        }
    }
}
