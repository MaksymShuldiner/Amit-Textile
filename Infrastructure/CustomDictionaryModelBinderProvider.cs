using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AmitTextile.Infrastructure
{
    public class CustomDictionaryModelBinderProvider:IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ILoggerFactory logger = context.Services.GetRequiredService<ILoggerFactory>();
            IModelBinder binder = new CustomDictionaryArrayModelBinder(new SimpleTypeModelBinder(typeof(Dictionary<string,List<string>>), logger));
            return context.Metadata.ModelType == typeof(Dictionary<string, List<string>>) ? binder : null;
        }
    }
}
