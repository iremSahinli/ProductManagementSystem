using ManagmentSystem.Domain.Core.BaseEntityConfiguration;
using ManagmentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Configurations
{
    public class UserProfileConfiguration : AuditableEntityConfiguration<UserProfile>
    {
        public override void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.Property(up => up.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(up => up.LastName).IsRequired().HasMaxLength(100);
            builder.Property(up => up.PhoneNumber).HasMaxLength(15);
            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.IdentityUserId);

            base.Configure(builder);
        }
    }
}