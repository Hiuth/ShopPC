using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface ICartRepository: IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task ClearCartAsync(string cartId);
        Task ClearAllCartAsync(string accountId);
    }
}
