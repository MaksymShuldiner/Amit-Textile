using System;

namespace AmitTextile.Domain
{
    public class FilterCharachteristics
    {
        public Guid FilterCharachteristicsId { get; set; }

        public Charachteristic Charachteristic { get; set; }
        
        public Guid CharachteristicId { get; set; }
    }
}