using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class ChildCategoriesViewModel
    {
        public List<Textile> Textiles { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public int SortingParams { get; set; }

        public ChildCategory Category { get; set; }

        public List<int> PagesCountList { get; set; }

        public string CookieValue { get; set; }

        public Dictionary<string, List<string>> FilterDictionary { get; set; }

        public string FilterQuery { get; set; }

        public List<Charachteristic> Charachteristic { get; set; }
    }
}