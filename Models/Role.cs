using System.ComponentModel.DataAnnotations;
namespace ShopPC.Models
{
    public class Role
    {
        [Key]
        public string roleName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        // 1 role có nhiều Permission
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
