using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPC.Models
{
    public class PcBuildItem
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string pcBuildId { get; set; } = string.Empty;
        public string productId { get; set; } = string.Empty;
        public int quantity { get; set; }

        // 1 pcBuildItem thuộc về 1 pcBuild
        [ForeignKey("pcBuildId")]
        public PcBuild pcBuild { get; set; } = null!;
        // 1 pcBuildItem thuộc về 1 product
        [ForeignKey("productId")]
        public Products product { get; set; } = null!;
    }
}
