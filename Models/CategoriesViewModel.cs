using System.Collections.Generic;
using AmitTextile.Domain;
using AmitTextile.Enums;

namespace AmitTextile.Models
{
    public class CategoriesViewModel
    {
        public List<ChildCategory> childCategories { get; set; }

        public List<Textile> Textiles { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public SortingParams SortingParams { get; set; }

    }
}