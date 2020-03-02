using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Item
    {
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }

        public int ItemsAmount { get; set; }

        public ICollection<ItemOrder> ItemOrders { get; set; }

        public Item()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }
}