using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitTextile.Domain
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }

        public int ItemsAmount { get; set; }

        public ICollection<ItemOrder> ItemOrders { get; set; }

        public bool isWithWholePrice
        {
            get => Convert.ToInt32(Textile.Charachteristics.FirstOrDefault(X => X.Name == "Оптовая цена").Value) >=
                   Textile.CostWithWholeCost;
        }
        public Guid? CartId { get; set; }

        public Cart Cart { get; set; }

        public bool IsWithWholesale { get; set; }

        public bool isBought { get; set; }

        public Item()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }
}