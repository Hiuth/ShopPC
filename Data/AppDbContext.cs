using Microsoft.EntityFrameworkCore;
using ShopPC.Models;
namespace ShopPC.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Account> Users { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<PaymentLogs> PaymentLogs { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<ProductImg> ProductImg { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
        public DbSet<WarrantyRecord> WarrantyRecords { get; set; }
        public DbSet<PcBuild> PcBuilds { get; set; }
        public DbSet<PcBuildItem> PcBuildItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleName, rp.PermissionName }); // Khóa chính của bảng trung gian

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleName);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionName);

            modelBuilder.Entity<PcBuild>().ToTable("PcBuild");
        }

        //public DbSet<RefreshToken> RefreshToken { get; set; }

    }
}
