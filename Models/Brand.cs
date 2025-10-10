using System.ComponentModel.DataAnnotations;
namespace ShopPC.Models
{
    public class Brand
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string brandName { get; set; } = string.Empty;

        // 1 brand có nhiều product
        public ICollection<Products> products { get; set; } = new List<Products>();
    }
}
