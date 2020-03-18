using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class SearchModel
    {
        public PageViewModel Model { get; set; }

        public List<TextileForFavViewModel> Textiles { get; set; }

        public List<int> PagesCounterList { get; set; }

        public string StringQuery { get; set; }
    }
}