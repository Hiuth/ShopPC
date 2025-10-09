using System.ComponentModel.DataAnnotations;

namespace ShopPC.Models
{
    public class Category
    {
        //? = thuộc tính có thể bằng null
        // [Required] = thuộc tính bắt buộc
        // [Key] = thuộc tính khóa chính

        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string categoryName { get; set; } = string.Empty;
        public string? iconImg { get; set; }


        // 1 category có nhiều subcategory
        public List<SubCategory> subCategories { get; set; } = new List<SubCategory>();

        // 1 category có nhiều product
        public List<Products> products { get; set; } = new List<Products>();
    }
}
