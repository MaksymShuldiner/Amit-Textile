﻿using System.Collections.Generic;
using AmitTextile.Domain;

namespace AmitTextile.Models
{
    public class ChildCategoriesViewModel
    {
        public List<Textile> Textiles { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public int SortingParams { get; set; }

        public ChildCategory Category { get; set; }

        public List<int> PagesCountList { get; set; }
    }
}