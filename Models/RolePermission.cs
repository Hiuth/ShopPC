namespace ShopPC.Models
{
    public class RolePermission
    {
        // Bảng trung gian cho mối quan hệ nhiều-nhiều
        public string RoleName { get; set; } = string.Empty;
        public Role Role { get; set; } = null!; // Tham chiếu đến Role

        public string PermissionName { get; set; } = string.Empty;
        public Permission Permission { get; set; } = null!; // Tham chiếu đến Permission
    }
}
