namespace ShopPC.DTO.Response
{
    public class CommentResponse
    {
        public string id { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public string accountName { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
    }
}
