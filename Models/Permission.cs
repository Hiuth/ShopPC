using System.ComponentModel.DataAnnotations;
namespace ShopPC.Models
{
    public class Permission
    {
        [Key]
        public string permissionName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        // 1 permission có nhiều role
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
