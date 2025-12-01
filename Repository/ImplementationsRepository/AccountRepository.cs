using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class AccountRepository : GenericReopository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _dbSet.AnyAsync(a => a.email == email);
        }

        public async Task<List<Account>> searchAccountAsync(string key)
        {
            return await _dbSet.Where(p => p.userName.Contains(key) || p.email.Contains(key)).ToListAsync();
        }

        public async Task<Account?> GetAccountById(string id)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.id == id);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.email == email);
        }
    }
}
