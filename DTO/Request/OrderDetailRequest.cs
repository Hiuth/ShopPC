using ShopPC.Models;
namespace ShopPC.DTO.Request
{
    public class OrderDetailRequest
    {
        public string orderId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}
