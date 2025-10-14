using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Products
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string productName { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int quantity { get; set; }
        public int stockQuantity { get; set; }
        public string? description { get; set; }
        public string? thumbnail { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public string brandId { get; set; } = string.Empty;
        public string subCategoryId { get; set; } = string.Empty;

        // 1 product thuộc về 1 brand
        [ForeignKey("brandId")]
        public Brand brand { get; set; } = null!;
        // 1 product thuộc về 1 subCategory
        [ForeignKey("subCategoryId")]
        public SubCategory subCategory { get; set; } = null!;

        // 1 product có nhiều productImg
        public ICollection<ProductImg> productImgs { get; set; } = new List<ProductImg>();

        // 1 productcó nhiều orderDetail
        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        // 1 product có nhiều ProductAttribute
        public ICollection<ProductAttribute> productAttributes { get; set; } = new List<ProductAttribute>();

        // 1 product có nhiều Cart
        public ICollection<Cart> carts { get; set; } = new List<Cart>();
    }
}
