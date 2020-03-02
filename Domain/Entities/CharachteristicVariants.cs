using System;

namespace AmitTextile.Domain
{
    public class CharachteristicVariants
    {
        public Guid CharachteristicVariantsId { get; set; }

        public Charachteristic Charachteristic { get; set; }

        public string Value { get; set; }

        public Guid CharachteristicId { get; set; }
    }
}