namespace ShopPC.DTO.Response
{
    public class ProductUnitResponse
    {
        public string id { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public string? imei { get; set; }
        public string? serialNumber { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
    }
}
