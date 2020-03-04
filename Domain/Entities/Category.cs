using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitTextile.Domain
{
    public class Category
    {
        
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<Textile> TextilesOfThisCategory { get; set; }

        public ICollection<ChildCategory> ChildCategories { get; set; }

        public Category()
        {
            TextilesOfThisCategory  = new List<Textile>();

            ChildCategories = new List<ChildCategory>();
        }
    }
}