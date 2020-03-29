using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitTextile.Domain
{
    public class ChildCategory
    {
        public Guid ChildCategoryId { get; set; }

        public Category Category { get; set; }

        [ForeignKey("Category")] 
        public Guid? CategoryId { get; set; }

        public string Name { get; set; }
        
        public ICollection<Textile> TextilesOfThisChildCategory { get; set; }

        public ChildCategory()
        {
            TextilesOfThisChildCategory = new List<Textile>();
        }
    }
}