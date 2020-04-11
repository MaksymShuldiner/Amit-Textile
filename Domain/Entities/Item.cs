using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public bool isWithWholePrice
        {
            get
            {
                bool res = false;
                try
                {
                    res = Convert.ToInt32(Textile.Charachteristics.FirstOrDefault(X => X.Name == "Оптовое количество")
                              .Value) <=
                          ItemsAmount;
                }
                catch
                {

                }

                return res;
            }
        }
        
        public Guid? CartId { get; set; }

        public Cart Cart { get; set; }


        public bool isBought { get; set; }

        public Item()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }
}