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
        public ICollection<SubCategory> subCategories { get; set; } = new List<SubCategory>();

        public ICollection<Brand> brands { get; set; } = new List<Brand>();

        public ICollection<Attributes> attributes { get; set; } = new List<Attributes>();
    }
}
