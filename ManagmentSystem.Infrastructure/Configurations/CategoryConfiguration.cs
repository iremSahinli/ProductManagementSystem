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
    public class CategoryConfiguration : AuditableEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(128);
            builder.Property(c => c.Description).IsRequired().HasMaxLength(128);
            base.Configure(builder);
        }
    }
}
