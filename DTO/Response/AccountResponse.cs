namespace ShopPC.DTO.Response
{
    public class AccountResponse
    {
        public string id { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public string gender { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string accountImg { get; set; } = string.Empty;
    }
}
