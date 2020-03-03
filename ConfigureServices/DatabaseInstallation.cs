using System;
using AmitTextile.Domain.Context;
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
        }
    }
}