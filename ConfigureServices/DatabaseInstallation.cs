using System;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AmitTextile.ConfigureServices
{
    public class DatabaseInstallation : IServiceIntallation
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AmitDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection").Replace("|ProjectFolder|", Environment.CurrentDirectory + "\\Domain")));
            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password = new PasswordOptions()
                    {
                        RequireDigit = true,
                        RequiredLength = 6,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequireNonAlphanumeric = false
                    };
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<AmitDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                
            });
            
           


        }
    }
}