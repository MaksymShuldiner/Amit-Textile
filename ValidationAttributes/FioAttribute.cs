using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AmitTextile.ValidationAttributes
{
    public class FioAttribute : ValidationAttribute
    {


        public FioAttribute()
        {
            
        }
        public override bool IsValid(object value)
        {
            Regex regex = new Regex(@"^(\s*)?((([а-яА-ЯёЁіІїЇa-zA-Z]+)(\s+))(([а-яА-ЯёЁіІїЇa-zA-Z]+)(\s+))(([а-яА-ЯёЁіІїЇa-zA-Z]+)(\s*)?))$");
            if (!regex.IsMatch(value.ToString()))
            {
                return false;
            }
            return true;
        }
    }
}
