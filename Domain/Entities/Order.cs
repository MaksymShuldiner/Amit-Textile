using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }
        
        public string CardNum { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Fio { get; set; }

        public string DepartmentName { get; set; }

        public bool isPickup { get; set; }
        public int DepartmentNum { get; set; }

        public bool isToAddress { get; set; }

        public string PhoneNumber { get; set; }

        public bool isPaidByCash { get; set; }

        public DateTime TimeCreated { get; set; }

        public int IsPaid { get; set; }

        public decimal Sum { get; set; }
        public bool isDelivery { get; set; }
        public ICollection<ItemOrder> ItemOrders { get; set; }

        public Order()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }
}