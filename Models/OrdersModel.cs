using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class OrdersModel
    {
        public PageViewModel Model { get; set; }

        public List<Order> Orders { get; set; }
    }
}