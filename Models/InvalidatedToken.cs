using System.ComponentModel.DataAnnotations;

namespace ShopPC.Models
{
    public class InvalidatedToken
    {
        [Key]
        public string id { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}
