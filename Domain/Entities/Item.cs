using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }

        public int ItemsAmount { get; set; }

        public ICollection<ItemOrder> ItemOrders { get; set; }

        public Guid? CartId { get; set; }

        public Cart Cart { get; set; }

        public bool isBought { get; set; }

        public Item()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }
}