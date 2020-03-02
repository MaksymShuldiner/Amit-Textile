using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Charachteristic
    {
        public Guid CharachteristicId { get; set; }

        public string Name { get; set; }

        public ICollection<CharachteristicValues> Values { get; set; }

        public ICollection<Textile> Textiles { get; set; }

        public Charachteristic()
        {
            Values = new List<CharachteristicValues>();
            Textiles = new List<Textile>();
        }

    }
}