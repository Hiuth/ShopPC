namespace ShopPC.DTO.Response
{
    public class ProductResponse
    {
        public string id { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int stockQuantity { get; set; }
        public string? description { get; set; }
        public string? thumbnail { get; set; }
        public DateTime createdAt { get; set; }
        public string status { get; set; } = string.Empty;
        public string brandId { get; set; } = string.Empty;
        public string brandName { get; set; } = string.Empty;
        public string subCategoryId { get; set; } = string.Empty;
        public string subCategoryName { get; set; } = string.Empty;
        public string warrantyPeriod { get; set; } = string.Empty;
    }
}
