namespace ShopPC.DTO.Response
{
    public class OrderDetailResponse
    {
        public string id { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}
