namespace ShopPC.DTO.Request
{
    public class ProductRequest
    {
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int quantity { get; set; }
        public int stockQuantity { get; set; }
        public string? description { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
