using System;

namespace AmitTextile.Domain
{
    public class ChildCommentReview
    {
        public Guid ChildCommentReviewId { get; set; }

        public string Text { get; set; }

        public ParentCommentReview ParentComment { get; set; }

        public Guid ParentCommentId { get; set; }

        public User Sender { get; set; }

        public Guid SenderId { get; set; }


    }
}