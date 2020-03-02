using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ItemOrder
    {
        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public Order Order { get; set; }

        public Item Item { get; set; }

        
    }
}