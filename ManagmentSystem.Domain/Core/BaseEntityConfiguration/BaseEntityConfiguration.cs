using ManagmentSystem.Domain.Core.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Core.BaseEntityConfiguration
{
    public class BaseEntityConfiguration<TEntity> :IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // otomatik olarak eklemek için kullanılan metot. BaseEntity new Guild yazmak ile aynı işlem
            builder.Property(x => x.Status).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedBy).IsRequired(false);
            builder.Property(x => x.ModifiedDate).IsRequired(false);

        }
    }
}
