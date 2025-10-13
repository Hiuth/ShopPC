namespace ShopPC.DTO.Response
{
    public class CartResponse
    {
        public string id { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public string accountId { get; set; } = string.Empty;
    }
}
