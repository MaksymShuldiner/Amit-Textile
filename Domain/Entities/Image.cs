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
    }
}