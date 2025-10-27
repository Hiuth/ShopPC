using System.ComponentModel.DataAnnotations;

namespace ShopPC.Models
{
    public class WarrantyUpdateLog
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime updateDate { get; set; }
        public int expiredCount { get; set; }
        public string? description { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
