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
        
        public DateTime LastTimeEmailForPassSent { get; set; }

        public DateTime LastTimePassChanged { get; set; }

        public DateTime LastTimeEmailForEmailSent { get; set; }

        public DateTime LastTimeEmailChanged { get; set; }
        public User()
        {
            ParentCommentQuestions = new List<ParentCommentQuestion>();
            ParentCommentReviews = new List<ParentCommentReview>();
            ChildCommentQuestions = new List<ChildCommentQuestion>();
        }

    }
}