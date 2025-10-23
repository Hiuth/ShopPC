namespace ShopPC.DTO.Response
{
    public class PcBuildItemResponse
    {
        public string id { get; set; } = string.Empty;
        public string pcBuildId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public string thumbnail { get; set; } = string.Empty;
    }
}
