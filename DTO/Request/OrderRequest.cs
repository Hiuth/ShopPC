using ShopPC.Models;
namespace ShopPC.DTO.Request
{
    public class OrderRequest
    {
        public string status { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public decimal totalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
    }
}
