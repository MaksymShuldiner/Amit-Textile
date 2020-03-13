using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AmitTextile.Infrastructure
{
    public class CustomDictionaryArrayModelBinder: IModelBinder
    {
        private readonly IModelBinder fallbackBinder;

        public CustomDictionaryArrayModelBinder(IModelBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var DictionaryValues = bindingContext.ValueProvider.GetValue("Filter");
            if (DictionaryValues == ValueProviderResult.None)
            {
                return fallbackBinder.BindModelAsync(bindingContext);
            }
            char[] dictChar = DictionaryValues.FirstValue.ToCharArray();
            string FilterName = "";
            for (int i = 0; i < dictChar.Length; i++)
            {
                if (dictChar[i+1] == ':')
                {
                    FilterName += dictChar[i];
                    int returnvalue = 0;
                    List<string> pushedList = new List<string>();
                    string pushedValue = "";
                    for (int j = i+2; j<dictChar.Length;j++)
                    {
                        
                        if (dictChar[j] != ';' && dictChar[j] != ',' && j!=dictChar.Length -1)
                        {
                            pushedValue += dictChar[j];
                        }
                        else if (dictChar[j] == ',')
                        {
                            pushedList.Add(pushedValue);
                            pushedValue = "";
                            returnvalue = j;
                            break;
                        }
                        else if(dictChar[j] == ';')
                        {
                            pushedList.Add(pushedValue);
                            pushedValue = "";
                           
                        }
                        else if (j == dictChar.Length - 1)
                        {
                            pushedValue += dictChar[j];
                            pushedList.Add(pushedValue);
                            pushedValue = "";
                            returnvalue = j;
                            break;
                        }
                        
                    }
                    dict.Add(FilterName, pushedList);
                    i = returnvalue;
                    FilterName = "";
                }
                else
                {
                    FilterName += dictChar[i];
                }
            }
            bindingContext.Result = ModelBindingResult.Success(dict);
            return Task.CompletedTask;
        }
    }
}
