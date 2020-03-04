using AmitTextile.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmitTextile.ConfigureServices
{
    public class OwnServicesInstaller : IServiceIntallation
    {
        public void Configure(IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddTransient<EmailService>();
        }
    }
}