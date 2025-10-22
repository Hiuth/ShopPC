namespace ShopPC.DTO.Response
{
    public class WarrantyRecordResponse
    {
        public string id { get; set; } = string.Empty;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public int warrantyPeriod { get; set; }
        public string orderId { get; set; } = string.Empty;
        public string productUnitId { get; set; } = string.Empty;
        public string? serialNumber { get; set; }
        public string? imei { get; set; }
    }
}
