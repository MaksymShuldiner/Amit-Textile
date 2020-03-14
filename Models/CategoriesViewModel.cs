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

        public int SortingParams { get; set; }

        public Category Category { get; set; }

        public List<int> PagesCountList { get; set; }

        public string CookieValue { get; set; }
        public string FilterQuery { get; set; }

        public List<FilterCharachteristics> Charachteristic { get; set; }
        public Dictionary<string,List<string>> FilterDictionary { get; set; }

    }
}