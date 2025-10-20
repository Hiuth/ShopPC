using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Report
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string accountId { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public string status { get; set; } = "Pending";

        [ForeignKey("accountId")]
        public Account account { get; set; } = null!;
    }
}
