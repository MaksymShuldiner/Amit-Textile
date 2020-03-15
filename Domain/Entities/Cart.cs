using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Cart
    {
        public Guid CartId { get; set; }

        public User User { get; set; }
        public Guid NonAuthorizedId { get; set; }
        public ICollection<Item> Items { get; set; }

        public Cart()
        {
            Items = new List<Item>();
        }

    }
}