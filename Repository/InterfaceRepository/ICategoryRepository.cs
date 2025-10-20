using ShopPC.Models;
using System.Linq.Expressions;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsCategoryNameUniqueAsync(string categoryName);
    }
}
