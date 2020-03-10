using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmitTextile.ConfigureServices
{
    public class MvcInstaller : IServiceIntallation
    {
        public void Configure(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddControllersWithViews();
            
        }
    }
}