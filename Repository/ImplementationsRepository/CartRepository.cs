using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class CartRepository: GenericReopository<Cart>,ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cart>> GetCartsByUserIdAsync(string userId)
        {
            return await _dbSet.Where(c => c.accountId == userId).ToListAsync();
        }

        public async Task ClearCartAsync(string cartId)
        {
            var cartItems = await _dbSet.Where(c => c.id == cartId).ToListAsync();
            _dbSet.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task ClearAllCart(string accountId)
        {
            var cart = await _dbSet.Where(c => c.accountId == accountId).ToListAsync();
            _dbSet.RemoveRange(cart);
            await _context.SaveChangesAsync();
        }
    }
}
