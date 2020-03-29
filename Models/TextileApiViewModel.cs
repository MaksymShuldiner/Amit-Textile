using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class TextileApiViewModel
    {
        public Guid TextileId { get; set; }


        [NotMapped]
        public decimal CostWithDiscount
        {
            get => Price * Convert.ToDecimal(Discount);
        }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Base64StrigImg { get; set; }
        public int WarehouseAmount { get; set; }
        public int Sold { get; set; }
        public bool IsOnDiscount { get; set; }

        public double Discount { get; set; }
        [NotMapped]
        public double Stars
        {
            get
            {
                double starssum = 0.0;
                double par = ParentCommentReviews.Count;
                foreach (var x in ParentCommentReviews)
                {
                    starssum += x.Stars;
                }

                if (ParentCommentReviews.Count == 0)
                {
                    par = 1;
                }
                return (starssum / par);
            }

        }

        public DateTime DateWhenAdded { get; set; }
     
        public int ViewsCounter { get; set; }
        public bool IsPopular { get; set; }
        public decimal PriceWithDiscount { get; set; }
        
        public ICollection<ParentCommentReview> ParentCommentReviews { get; set; }
    }
}