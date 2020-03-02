using System;

namespace AmitTextile.Domain
{
    public class CharachteristicValues
    {
        public Guid CharachteristicValuesId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
        
        public Textile Textile { get; set; }

        public Guid TextileId { get; set; }
    }
}