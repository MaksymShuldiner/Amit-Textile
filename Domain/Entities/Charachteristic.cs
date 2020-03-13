using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Charachteristic
    {
        public Guid CharachteristicId { get; set; }

        public string Name { get; set; }

        public ICollection<CharachteristicVariants> Values { get; set; }

        public FilterCharachteristics FilterCharachteristics { get; set; }

        public Charachteristic()
        {
            Values = new List<CharachteristicVariants>();
        }

    }
}