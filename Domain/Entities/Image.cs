using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitTextile.Domain
{
    public class Image
    {
        public Guid ImageId { get; set; }

        public string Name { get; set; }

        public byte[] ByteImg { get; set; }

        public Guid? TextileId { get; set; }
        [ForeignKey("TextileId")]
        public Textile Textile { get; set; }
        [ForeignKey("MainTextile")]
        public Guid? MainTextileId { get; set; }
        
        public Textile MainTextile { get; set; }
        public Slider Slider { get; set; }

        public Guid? SliderId { get; set; }
    }
}