using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class ProductImg
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();

        public string imgUrl { get; set; } = string.Empty;

        public string productId { get; set; } = string.Empty;

        // 1 productImg thuộc về 1 product
        [ForeignKey("productId")]
        public Products product { get; set; } = null!;


    }
}
