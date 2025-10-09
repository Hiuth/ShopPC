using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class ProductAttribute
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string productId { get; set; } = string.Empty;
        public string attributeId { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;

        // 1 productAttribute thuộc về 1 product
        [ForeignKey("attributeId")]
        public Attribute attribute { get; set; } = null!;

        [ForeignKey("productId")]
        public Products product { get; set; } = null!;
    }
}
