using ShopPC.Models;
namespace ShopPC.DTO.Request
{
    public class OrderDetailRequest
    {
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}
