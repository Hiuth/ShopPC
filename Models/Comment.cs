using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Comment
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string productId { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int totalLike { get; set; } = 0;

        [ForeignKey("productId")]
        public Products product { get; set; } = null!;

        [ForeignKey("accountId")]
        public Account account { get; set; } = null!;
    }
}
