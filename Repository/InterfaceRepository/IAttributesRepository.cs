using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IAttributesRepository: IGenericRepository<Attributes>
    {
        Task<List<Attributes>> GetAttributesBySubCategoryId(string subCategoryId);
    }
}
