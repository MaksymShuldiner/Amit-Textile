using System;

namespace AmitTextile.Domain
{
    public class ChildCommentQuestion
    {
        public Guid ChildCommentQuestionId { get; set; }

        public string Text { get; set; }

        public ParentCommentQuestion ParentComment { get; set; }

        public Guid ParentCommentId { get; set; }

        public User Sender { get; set; }

        public string SenderId { get; set; }



    }
}