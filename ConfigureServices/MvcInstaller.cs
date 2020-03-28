 using AmitTextile.Infrastructure;
 using Microsoft.AspNetCore.Authentication.Cookies;
 using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmitTextile.ConfigureServices
{
    public class MvcInstaller : IServiceIntallation
    {
        public void Configure(IServiceCollection services, IConfiguration Configuration)
        {
            
            services.AddControllersWithViews(opts =>
            {
                opts.ModelBinderProviders.Insert(0, new CustomDictionaryModelBinderProvider());
            }).AddNewtonsoftJson(options=>options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            
        }
    }
}