﻿using System;
using System.Collections.Generic;

namespace AmitTextile.Domain
{
    public class Textile
    {
        public Guid TextileId { get; set; }

        public ICollection<CharachteristicValues> Charachteristics { get; set; }

        public ICollection<Image> Images { get; set; }

        public int WarehouseAmount { get; set; }

        public int Stars { get; set; }

        public Textile()
        {
            Charachteristics = new List<CharachteristicValues>();
            Images = new List<Image>();
        }
    }
}