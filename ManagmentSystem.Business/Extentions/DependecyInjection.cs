using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Business.Services.CategoryServices;
using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Business.Services.UserProfileServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Extentions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)

        {

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            return services;
        }

    }
}
