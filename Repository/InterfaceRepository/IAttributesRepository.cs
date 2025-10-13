using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IAttributesRepository: IGenericRepository<Attributes>
    {
        Task<IEnumerable<Attributes>> GetAttributesByCategoryId(string categoryId);
    }
}
