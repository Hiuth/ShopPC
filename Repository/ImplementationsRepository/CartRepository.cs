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

        public async Task<List<Cart>> GetCartByAccountIdAsync(string accountId)
        {
            return await _dbSet
                .Include(c => c.product)
                .Where(c => c.accountId == accountId).ToListAsync();
        }

        public async Task<Cart?> GetCartByProductIdAndProductIdAsync(string accountId, string productId)
        {
            return await _dbSet
                .Include(c => c.product)
                .FirstOrDefaultAsync(c => c.accountId == accountId && c.productId == productId);
        }

        public async Task<bool> IsProductInCartAsync(string accountId, string productId)
        {
            return await _dbSet.AnyAsync(c => c.accountId == accountId && c.productId == productId);
        }

        public async Task ClearCartAsync(string cartId)
        {
            var cartItems = await _dbSet.Where(c => c.id == cartId).ToListAsync();
            _dbSet.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task ClearAllCartAsync(string accountId)
        {
            var cart = await _dbSet.Where(c => c.accountId == accountId).ToListAsync();
            _dbSet.RemoveRange(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartByCartIdAsync(string cartId)
        {
            return await _dbSet
                .Include(c => c.product)
                .FirstOrDefaultAsync(c => c.id == cartId);
        }
    }
}
