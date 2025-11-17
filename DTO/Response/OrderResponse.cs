namespace ShopPC.DTO.Response
{
    public class OrderResponse
    {
        public string id { get; set; } = string.Empty;
        public DateTime orderDate { get; set; }
        public string status { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public decimal totalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public bool isPaid { get; set; }
    }
}
