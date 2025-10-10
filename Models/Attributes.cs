using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Attributes
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string attributeName { get; set; } = string.Empty;
        public string subCategoryId { get; set; } = string.Empty;
        // 1 attribute thuộc về 1 subCategory
        [ForeignKey("subCategoryId")]
        public SubCategory subCategory { get; set; } = null!;

        // 1 attribute có nhiều ProductAttribute
        public ICollection<ProductAttribute> productAttributes { get; set; } = new List<ProductAttribute>();
    }
}
