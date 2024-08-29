using ManagmentSystem.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;

namespace ManagmentSystem.Presentation.Extentions
{


    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {



            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
    }

}
