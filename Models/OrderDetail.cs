using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class OrderDetail
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string orderId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }

        // 1 orderDetail thuộc về 1 order
        [ForeignKey("orderId")]
        public Order order { get; set; } = null!;

        // 1 orderDetail thuộc về 1 product
        [ForeignKey("productId")]
        public Products product { get; set; } = null!;
    }
}
