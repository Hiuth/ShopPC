using System.ComponentModel.DataAnnotations;
namespace ShopPC.Models
{
    public class Role
    {
        [Key]
        public string permissionName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        // 1 role có nhiều Permission

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
