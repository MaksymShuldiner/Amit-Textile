using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class TextileViewModel
    {
        public Textile Textile { get; set; }

        public List<ParentCommentReview> parentCommentReviews { get; set; }

        public List<ParentCommentQuestion> parentCommentQuestions { get; set; }
    }
}