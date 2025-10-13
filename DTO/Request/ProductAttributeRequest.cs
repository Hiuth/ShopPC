namespace ShopPC.DTO.Request
{
    public class ProductAttributeRequest
    {
        public string productId { get; set; } = string.Empty;
        public string attributeId { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
    }
}
