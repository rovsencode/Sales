using FirelloProject.DAL;
using FirelloProject.Helpers;
using FirelloProject.Models;
using FirelloProject.Services.Basket;
using FirelloProject.Services.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirelloProject
{
    public static class ServiceRegistration
    {
        public static void FirelloServiceRegistration(this IServiceCollection services)
        {
          
            services.AddHttpContextAccessor();

            services.AddScoped<IBasketProductCount, BasketProductCount>();
            services.AddScoped<IProduct,ProductServices>();
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit=true;
                options.Password.RequireLowercase=true;
                options.Password.RequireUppercase=true;

                options.User.RequireUniqueEmail=true;

                options.Lockout.AllowedForNewUsers=true;
                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders()
              .AddErrorDescriber<CustomIdentityErrorDescriper>();

        } 
    }
}

