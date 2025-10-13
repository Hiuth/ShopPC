using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class AccountRepository: GenericReopository<Account>,IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _dbSet.AnyAsync(a => a.email == email);
        }
    }
}
