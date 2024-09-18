using AspNetCoreHero.ToastNotification;
using ManagmentSystem.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Reflection;

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

            services.AddControllersWithViews(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix, opt => opt.DataAnnotationLocalizerProvider = (type, factory) =>
            {
                //var assemblyName = typeof(SharedResource).GetTypeInfo().Assembly.GetName().Name!;
                var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
                //The exclamation mark (!) at the end of typeof(SharedResource).GetTypeInfo().Assembly.FullName! is a null-forgiving operator in C#. It is used to tell the compiler that you are certain the expression will not be null, thus suppressing any nullability warnings.
                return factory.Create(nameof(SharedResources), assemblyName.Name!);
            }); //SearchedLocation = "30-CultureInfoo.Resources._30_CultureInfoo.SharedResource" 

            services.AddLocalization(opt => opt.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCulture = new List<CultureInfo>()
                {
                    new CultureInfo("en"),
                    new CultureInfo("tr")
                };
                opt.DefaultRequestCulture = new RequestCulture("tr");//muhakkak bir cookie istiyor
                opt.SupportedUICultures = supportedCulture;
                opt.SupportedCultures = supportedCulture;

                //opt.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

            });

            // services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
    }

}
