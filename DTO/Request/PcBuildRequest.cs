namespace ShopPC.DTO.Request
{
    public class PcBuildRequest
    {
        public string productName { get; set; } = string.Empty;
        public decimal? price { get; set; }
        public string? description { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
