using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;

namespace ShopPC.Service.ImplementationsService
{
    public class BrandService: IBrandService
    {
        private readonly IBrandRepository _brandReopsitory;
        private readonly ICategoryRepository _categoryRepository;

        public BrandService(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _brandReopsitory = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<BrandResponse> createBrand(string categoryId, BrandRequest request)
        {
            if (await _brandReopsitory.IsBrandNameUniqueAsync(request.brandName))
            {
                throw new AppException(ErrorCode.BRAND_ALREADY_EXISTS);
            }
            if(!await _categoryRepository.ExistsAsync(categoryId))
            {
                 throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }

            var brand = BrandMapper.toBrand(request);
            var createdBrand = await _brandReopsitory.AddAsync(brand);
            return BrandMapper.toBrandResponse(createdBrand);
        }

        public async Task<BrandResponse> updateBrand(string brandId, string categoryId, BrandRequest request)
        {
             var brand = await _brandReopsitory.GetByIdAsync(brandId) ??
                throw new AppException(ErrorCode.BRAND_NOT_EXISTS);

            if(!String.IsNullOrWhiteSpace(categoryId))
            {
                if (!await _categoryRepository.ExistsAsync(categoryId))
                {
                    throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
                }
                brand.categoryId = categoryId;
            }
   
                
            if(!String.IsNullOrWhiteSpace(request.brandName))
            {
                if (await _brandReopsitory.IsBrandNameUniqueAsync(request.brandName))
                {
                    throw new AppException(ErrorCode.BRAND_ALREADY_EXISTS);
                } brand.brandName = request.brandName;

            }
            await _brandReopsitory.UpdateAsync(brand);
            return BrandMapper.toBrandResponse(brand);
        }

        public async Task<List<BrandResponse>> getAllBrand()
        {
            var brands = await _brandReopsitory.GetAllAsync();
            return brands.Select(BrandMapper.toBrandResponse).ToList();
        }

        public async Task<BrandResponse> getBrandById(string id)
        {
            var brand = await _brandReopsitory.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.BRAND_NOT_EXISTS);
            return BrandMapper.toBrandResponse(brand);
        }

        public async Task<List<BrandResponse>> getBrandByCategoryId(string id)
        {
            if(!await _categoryRepository.ExistsAsync(id))
            {
                 throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }
            var brands = await _brandReopsitory.GetBrandByCategoryIdAsync(id);
            return brands.Select(BrandMapper.toBrandResponse).ToList();
        }
    }
}
