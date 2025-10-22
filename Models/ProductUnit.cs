using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPC.Models
{
    [Index(nameof(imei), IsUnique = true)]
    [Index(nameof(serialNumber), IsUnique = true)]
    public class ProductUnit
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string productId { get; set; } = string.Empty;
        public string? imei { get; set; }
        public string? serialNumber { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;

        [ForeignKey("productId")]
        public Products product { get; set; } = null!;

        //1 productUnit có một warrantyRecord
        public WarrantyRecord warrantyRecord { get; set; } = null!;
    }
}
