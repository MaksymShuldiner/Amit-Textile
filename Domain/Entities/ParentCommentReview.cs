using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ParentCommentReview
    {
        public Guid ParentCommentReviewId { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public string SenderId { get; set; }

        public double Stars { get; set; }
        
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }
    
        public string Fio { get; set; }
        public string Advantages { get; set; }

        public string DrawBacks { get; set; }
        public ParentCommentReview()
        {
            
        }
        public DateTime DatePosted { get; set; }


    }
}