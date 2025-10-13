using ShopPC.Models;
using System.Linq.Expressions;
namespace ShopPC.Repository.InterfaceRepository
{
    public interface IProductRepository: IGenericRepository<Products>
    {
        Task<IEnumerable<Products>> GetProductsByBrandIdAsync(string brandId);

        Task<IEnumerable<Products>> GetProductsBySubCategoryIdAsync(string subCategoryId);

        Task<IEnumerable<Products>> SearchProductsAsync(string searchTerm);

        Task<IEnumerable<Products>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        Task<IEnumerable<Products>> GetProductsByCategoryIdAsync(string categoryId);

        //Task<IEnumerable<Products>> GetTopRatedProductsAsync(int count);
        //Task<IEnumerable<Products>> GetMostPopularProductsAsync(int count);
        //Task<IEnumerable<Products>> GetProductsByAttributeAsync(string attributeName, string attributeValue);
        //Task<IEnumerable<Products>> GetProductsWithIncludesAsync(string includeProperties);
    }
}