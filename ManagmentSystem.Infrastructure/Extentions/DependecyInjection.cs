using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Infrastructure.Extentions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));
            });

        
            AdminSeed.AdminSeedAsync(configuration).GetAwaiter().GetResult();

            return services;
        }


    }
}
