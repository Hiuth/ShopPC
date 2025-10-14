using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IBrandService
    {
        Task<BrandResponse> createBrand(string categoryId,BrandRequest request);
        Task<BrandResponse> updateBrand(string brandId,string? categoryId ,BrandRequest request);
        //Task<bool> deleteBrand(string id);
        Task<List<BrandResponse>> getAllBrand();
        Task<BrandResponse> getBrandById(string id);
        Task<List<BrandResponse>> getBrandByCategoryId(string id);
    }
}
