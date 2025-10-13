namespace ShopPC.DTO.Request
{
    public class SubCategoryRequest
    {
        public string subCategoryName { get; set; } = string.Empty;
        public string? subCategoryImg { get; set; }
        public string? description { get; set; }
        public string categoryId { get; set; } = string.Empty;
    }
}
