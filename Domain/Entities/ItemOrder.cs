using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ItemOrder
    {
        public Guid OrderId { get; set; }

        public Guid ItemId { get; set; }

        public Order Order { get; set; }

        public Item Item { get; set; }

        
    }
}