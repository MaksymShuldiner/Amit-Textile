using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class TextileViewModel
    {
        public Textile Textile { get; set; }

        public List<ParentCommentReview> parentCommentReviews { get; set; }

        public List<ParentCommentQuestion> parentCommentQuestions { get; set; }

        public string Section { get; set; }

        public string Fio { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public List<int> PagesCount { get; set; } = new List<int>();
    }
}