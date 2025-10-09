using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPC.Models
{
    public class SubCategory
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string subCategoryName { get; set; } = string.Empty;
        public string? subCategoryImg { get; set; }
        public string? description { get; set; }

        public string categoryId { get; set; } = string.Empty;

        // 1 subCategory có nhiều product
        [ForeignKey("categoryId")]
        public Category category { get; set; } = null!;

        //1 subCategory có nhiều product
        public ICollection<Products> products { get; set; } = new List<Products>();

        //1 subCategory có nhiều attribute
        public ICollection<Attribute> attributes { get; set; } = new List<Attribute>();
    }
}
