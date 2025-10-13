namespace ShopPC.DTO.Request
{
    public class CartRequest
    {
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public string accountId { get; set; } = string.Empty;
    }
}
