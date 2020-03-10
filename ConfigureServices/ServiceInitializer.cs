using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmitTextile.ConfigureServices
{
    public static class ServiceInitializer
    {
        public static void DefaultConfigure(this IServiceCollection services, IConfiguration Configuration)
        {
           IEnumerable<IServiceIntallation> serviceCollection = Assembly.GetAssembly(typeof(Startup)).GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && typeof(IServiceIntallation).IsAssignableFrom(x)).Select(x => Activator.CreateInstance(x)).Cast<IServiceIntallation>().ToList();
           foreach (var x in serviceCollection)
           {
               x.Configure(services, Configuration);
           }
        }
    }
}