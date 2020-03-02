using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class ParentCommentQuestion
    {
        public Guid ParentCommentQuestonId { get; set; }

        public ICollection<ChildCommentQuestion> ChildComments { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public Guid SenderId { get; set; }

        public ParentCommentQuestion()
        {
            ChildComments = new List<ChildCommentQuestion>();
        }

    }
}