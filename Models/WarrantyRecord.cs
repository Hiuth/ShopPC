using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPC.Models
{
    public class WarrantyRecord
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string productUnitId { get; set; } = string.Empty;

        [ForeignKey("productId")]
        public Products product { get; set; } = null!;

        [ForeignKey("orderId")]
        public Order order { get; set; } = null!;

        [ForeignKey("productUnitId")]
        public ProductUnit productUnit { get; set; } = null!;
    }
}
