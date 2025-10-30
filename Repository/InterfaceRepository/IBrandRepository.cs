using ShopPC.Models;
using System.Linq.Expressions;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IBrandRepository: IGenericRepository<Brand>
    {
        Task<bool> IsBrandNameUniqueAsync(string brandName);
    }
}
