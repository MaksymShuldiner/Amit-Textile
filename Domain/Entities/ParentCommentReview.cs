﻿using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ParentCommentReview
    {
        public Guid ParentCommentReviewId { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public string SenderId { get; set; }

        public int Stars { get; set; }
        
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }
        public ParentCommentReview()
        {
            
        }
        public DateTime DatePosted { get; set; }


    }
}