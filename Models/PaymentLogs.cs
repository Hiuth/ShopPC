using System.ComponentModel.DataAnnotations;
namespace ShopPC.Models
{
    public class PaymentLogs
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string orderId { get; set; } = string.Empty;
        public DateTime paymentDate { get; set; } = DateTime.Now;
        public decimal amount { get; set; }
        public string paymentMethod { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string transactionId { get; set; } = string.Empty;
    }
}
