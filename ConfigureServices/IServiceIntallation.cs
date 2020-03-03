using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmitTextile.ConfigureServices
{
    public interface IServiceIntallation
    {
        public void Configure(IServiceCollection collection, IConfiguration configuration);

    }
}