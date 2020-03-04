using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AmitTextile.Domain
{
    public class User : IdentityUser
    {
        public Cart Cart { get; set; }

        public Guid CartId { get; set; }

        public string Address { get; set; }

        public string Fio { get; set; }

        public ICollection<UserChosenTextile> UserChosenTextiles { get; set; }

        public ICollection<ParentCommentReview> ParentCommentReviews { get; set; }

        public ICollection<ParentCommentQuestion> ParentCommentQuestions { get; set; }
        public ICollection<ChildCommentQuestion> ChildCommentQuestions { get; set; }
        public ICollection<ChildCommentReview> ChildCommentReviews { get; set; }

        public User()
        {
            ParentCommentQuestions = new List<ParentCommentQuestion>();
            ParentCommentReviews = new List<ParentCommentReview>();
            ChildCommentReviews = new List<ChildCommentReview>();
            ChildCommentQuestions = new List<ChildCommentQuestion>();
        }

    }
}