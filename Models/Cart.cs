using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Cart
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }
        public string accountId { get; set; } = string.Empty;

        [ForeignKey("accountId")]
        public Account account { get; set; } = null!;

        [ForeignKey("productId")]
        public Products product { get; set; } = null!;

    }
}
