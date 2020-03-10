using System;

namespace AmitTextile.Domain
{
    public class UserChosenTextile
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public Guid TextileId { get; set; }

        public Textile Textile { get; set; }
    }
}