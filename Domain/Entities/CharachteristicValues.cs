using System;

namespace AmitTextile.Domain
{
    public class CharachteristicValues
    {
        public Guid CharachteristicValuesId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
        public Charachteristic Charachteristic { get; set; }

        public Guid CharachteristicId { get; set; }
    }
}