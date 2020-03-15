using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitTextile.Domain
{
    public class ChildCommentQuestion
    {
        public Guid ChildCommentQuestionId { get; set; }

        public string Text { get; set; }

        public ParentCommentQuestion ParentComment { get; set; }

        public Guid ParentCommentId { get; set; }
        [ForeignKey("SenderId")]
        public User Sender { get; set; }

        public string SenderId { get; set; }

        public DateTime DatePosted { get; set; }

        public Textile Textile { get; set; }

        public string Fio { get; set; }

        public Guid TextileId { get; set; }


    }
}