
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        public string? Category { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}
