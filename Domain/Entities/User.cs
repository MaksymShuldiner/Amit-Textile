using System;
using Microsoft.AspNetCore.Identity;

namespace AmitTextile.Domain
{
    public class User : IdentityUser
    {
        public Cart Cart { get; set; }

        public Guid CartId { get; set; }

        public string Address { get; set; }

        public string Fio { get; set; }


    }
}