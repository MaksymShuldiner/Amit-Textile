using System.Collections.Generic;

namespace AmitTextile.Models
{
    public class CatAddModel
    {
        public string Name { get; set; }

        public List<string> ChildCategoryId { get; set; } = new List<string>();
    }
}