using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Slider
    {
        public Guid SliderId { get; set; }

        public string SliderName { get; set; }

        public List<Image> Images { get; set; }
    }
}