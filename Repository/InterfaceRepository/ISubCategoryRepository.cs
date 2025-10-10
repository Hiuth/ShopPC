using System.Linq.Expressions;
using ShopPC.Models;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        Task<bool> IsSubCategoryNameUniqueAsync(string subCategoryName);

        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(string categoryId);
    }
}
