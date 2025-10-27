using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Models;
using BC = BCrypt.Net.BCrypt;

namespace ShopPC.Data
{
    public class DatabaseInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(AppDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
                var adminExists = await _context.Users.AnyAsync(a => a.userName == "admin");
                if (!adminExists)
                {
                    var adminAccount = new Account
                    {
                        id = Guid.NewGuid().ToString(),
                        userName = "admin",
                        email = "admin",
                        password = BC.HashPassword("admin"),
                        roleName = "ADMIN",
                        gender = "Male",
                        phoneNumber = "0000000000",
                        address = "System Administrator",
                        accountImg = "https://res.cloudinary.com/dggt29zsn/image/upload/v1761529832/Funny_Cat_Faces_upxnd6.jpg", // Default avatar
                        createdAt = DateTime.Now
                    };

                    await _context.Users.AddAsync(adminAccount);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Admin account created successfully with username: admin and password: admin");
                }
                else
                {
                    _logger.LogInformation("Admin account already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }

        }

    }
}
  
