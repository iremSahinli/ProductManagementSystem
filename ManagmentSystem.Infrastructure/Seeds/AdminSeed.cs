using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Enums;
using ManagmentSystem.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Seeds
{
    public static class AdminSeed
    {
        private const string adminEmail = "admin@admin.com";
        private const string adminPassword = "password+0";

        public static async Task AdminSeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextBuilder.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));
            AppDbContext context = new AppDbContext(dbContextBuilder.Options);

            if (!context.Roles.Any())
            {
                await AddRolesAsync(context);
            }
            if (!context.Users.Any(user => user.Email == adminEmail))
            {
                await AddAdminAsync(context);
            }


        }

        private static async Task AddAdminAsync(AppDbContext context)
        {
            IdentityUser user = new IdentityUser()
            {
                Email = adminEmail,
                EmailConfirmed = true,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpperInvariant()
            };
            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, adminPassword);
            await context.Users.AddAsync(user);

            var adminRoleId = context.Roles.FirstOrDefault(role => role.Name == Roles.Admin.ToString()).Id;

            IdentityUserRole<string> userRole = new IdentityUserRole<string>()
            {
                RoleId = adminRoleId,
                UserId = user.Id
            };
            await context.UserRoles.AddAsync(userRole);

            await context.Admins.AddAsync(new Admin()
            {
                FirstName = "admin",
                LastName = "admin",
                Email = adminEmail,
                IdentityId = user.Id
            });
            await context.SaveChangesAsync();
        }

        private static async Task AddRolesAsync(AppDbContext context)
        {
            string[] roles = Enum.GetNames(typeof(Roles));

            for (int i = 0; i < roles.Length; i++)
            {
                if (await context.Roles.AnyAsync(role => role.Name == roles[i]))
                {
                    continue;
                }
                IdentityRole role = new IdentityRole()
                {
                    Name = roles[i],
                    NormalizedName = roles[i].ToUpperInvariant()
                };
                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();



            }

        }
    }
}
