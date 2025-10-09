using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Notification
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string accountId { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public bool isRead { get; set; } = false;

        // 1 notification thuộc về 1 account
        [ForeignKey("accountId")]
        public Account account { get; set; } = null!;
    }
}
