using AspNetCoreHero.ToastNotification;
using ManagmentSystem.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;

namespace ManagmentSystem.Presentation.Extentions
{


    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddNotyf(config =>
            {
                config.HasRippleEffect = true;
                config.DurationInSeconds = 3;
                config.Position = NotyfPosition.BottomRight;
                config.IsDismissable = true;
            });


            // services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
    }

}
