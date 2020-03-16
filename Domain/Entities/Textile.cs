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
                return (starssum/par);
            }
            
        }

        public DateTime DateWhenAdded { get; set; }

        public int ViewsCounter { get; set; }
        public bool IsPopular { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid? ChildCategoryId { get; set; }

        public string Description { get; set; }

        public ICollection<ChildCommentQuestion> ChildCommentQuestions { get; set; }
        public ICollection<ParentCommentReview> ParentCommentReviews { get; set; }
        public ICollection<ParentCommentQuestion> ParentCommentQuestions { get; set; }
        public ChildCategory ChildCategory { get; set; }
        public Textile()
        {
            Charachteristics = new List<CharachteristicValues>();
            ChildCommentQuestions = new List<ChildCommentQuestion>();
            ParentCommentQuestions = new List<ParentCommentQuestion>();
            ParentCommentReviews = new List<ParentCommentReview>();
            Images = new List<Image>();
        }
    }
}