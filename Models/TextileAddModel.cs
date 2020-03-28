using Microsoft.AspNetCore.Http;

namespace AmitTextile.Models
{
    public class TextileAddModel
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public decimal Price { get; set; }

        public int WarehouseAmount { get; set; }

        public int Sold { get; set; }
        public bool IsOnDiscount { get; set; }

        public double Discount { get; set; }

        public string Description { get; set; }

        public IFormFile MainFile { get; set; }

        public IFormFile[] Files { get; set; }

    }
}