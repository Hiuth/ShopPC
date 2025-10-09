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

    }
}
