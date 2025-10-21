using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface ICartRepository: IGenericRepository<Cart>
    {
        Task<List<Cart>> GetCartByAccountIdAsync(string accountId);
        Task<Cart?> GetCartByProductIdAndProductIdAsync(string accountId, string productId);
        Task<bool> IsProductInCartAsync(string accountId, string productId);
        Task<Cart?> GetCartByCartIdAsync(string cartId);
        Task ClearCartAsync(string cartId);
        Task ClearAllCartAsync(string accountId);
    }
}
