namespace ShopPC.DTO.Response
{
    public class OrderDetailResponse
    {
        public string id { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}
