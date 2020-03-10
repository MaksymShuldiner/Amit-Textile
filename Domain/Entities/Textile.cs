using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitTextile.Domain
{
    public class Textile
    {
        public Guid TextileId { get; set; }

        public ICollection<CharachteristicValues> Charachteristics { get; set; }

        [NotMapped]
        public decimal CostWithDiscount
        {
            get => Price * Convert.ToDecimal(Discount);
        }
        public string Name { get; set; }
        public ICollection<Image> Images { get; set; }  

        public string Status { get; set; }

        public decimal Price { get; set; }

        public ICollection<UserChosenTextile> UserChosenTextiles { get; set; }
        public int WarehouseAmount { get; set; }

        public int Sold { get; set; }
        public bool IsOnDiscount { get; set; }

        public double Discount { get; set; }
        public double Stars { get; set; }

        public DateTime DateWhenAdded { get; set; }

        public int ViewsCounter { get; set; }
        public bool IsPopular { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid? ChildCategoryId { get; set; }

        public ChildCategory ChildCategory { get; set; }
        public Textile()
        {
            Charachteristics = new List<CharachteristicValues>();
            Images = new List<Image>();
        }
    }
}