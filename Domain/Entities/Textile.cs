using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Textile
    {
        public Guid TextileId { get; set; }

        public ICollection<CharachteristicValues> Charachteristics { get; set; }

        public double Cost { get; set; }

        public double CostWithDiscount
        {
            get => Cost * Discount; set => CostWithDiscount = value;
        }

        public ICollection<Image> Images { get; set; }  

        public string Status { get; set; }

        public int WarehouseAmount { get; set; }

        public bool IsOnDiscount { get; set; }

        public double Discount { get; set; }
        public int Stars { get; set; }

        public DateTime DateWhenAdded { get; set; }

        public int ViewsCounter { get; set; }
        public bool IsPopular { get; set; }
        public Textile()
        {
            Charachteristics = new List<CharachteristicValues>();
            Images = new List<Image>();
        }
    }
}