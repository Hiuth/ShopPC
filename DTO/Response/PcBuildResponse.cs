namespace ShopPC.DTO.Response
{
    public class PcBuildResponse
    {
        public string id { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public string? description { get; set; }
        public string status { get; set; } = string.Empty;
        public string thumbnail { get; set; } = string.Empty;
        public string subCategoryId { get; set; } = string.Empty;
        public string subCategoryName { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public string categoryName { get; set; } = string.Empty;
    }
}
