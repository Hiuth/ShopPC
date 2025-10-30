using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        Task<bool> IsSubCategoryNameUniqueAsync(string subCategoryName);

        Task<SubCategory?> GetSubCategoryByIdAsync(string subCategoryId);

        Task<List<SubCategory>> GetAllSubCategoryAsync();

        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(string categoryId);
    }
}
