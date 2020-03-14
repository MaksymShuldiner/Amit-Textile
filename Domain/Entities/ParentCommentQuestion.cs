using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ParentCommentQuestion
    {
        public Guid ParentCommentQuestionId { get; set; }

        public ICollection<ChildCommentQuestion> ChildComments { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public string SenderId { get; set; }

        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }
        public ParentCommentQuestion()
        {
            ChildComments = new List<ChildCommentQuestion>();
        }

        public DateTime DatePosted { get; set; }
    }
}