using System;

namespace AmitTextile.Domain
{
    public class Image
    {
        public Guid ImageId { get; set; }

        public string Name { get; set; }

        public byte[] ByteImg { get; set; }

        public Guid TextileId { get; set; }

        public Textile Textile { get; set; }

        public Slider Slider { get; set; }

        public Guid SliderId { get; set; }
    }
}