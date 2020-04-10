using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization.Internal;

namespace AmitTextile.Models
{
    public class TextileAddModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public decimal CostWithWholeCost { get; set; }

        public decimal Price { get; set; }

        public int WarehouseAmount { get; set; }
        public bool IsOnDiscount { get; set; }

        public decimal Discount { get; set; }

        public string[] CharacsNames { get; set; }

        public string[] CharacsValues { get; set; }

        public string CategoryId { get; set; }

        public string ChildCategoryId { get; set; }

        public string Description { get; set; }

        public IFormFile MainFile { get; set; }

        public IFormFile[] Files { get; set; }

    }
}