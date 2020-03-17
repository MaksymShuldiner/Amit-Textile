using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class ShowFavouriteModel
    {
        public List<Textile> Textiles { get; set; }

        public PageViewModel Model { get; set; }
    }
}