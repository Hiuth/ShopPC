using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;
using System.Threading.Tasks;


namespace ShopPC.Service.ImplementationsService
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICloudinaryService _cloudinaryService;
        public ProductService(IProductRepository productRepository, IBrandRepository brandRepository, ISubCategoryRepository subCategoryRepository, ICloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _subCategoryRepository = subCategoryRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ProductResponse> CreateProduct(string brandId, string subCategoryId, ProductRequest request, IFormFile file)
        {
            if (!await _brandRepository.ExistsAsync(brandId))
            {
                throw new AppException(ErrorCode.BRAND_NOT_EXISTS);
            }

            if (!await _subCategoryRepository.ExistsAsync(subCategoryId))
            {
                throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);
            }

            var product = ProductMapper.toProducts(request);
            product.brandId = brandId;
            product.subCategoryId = subCategoryId;
            product.thumbnail = await _cloudinaryService.UploadImageAsync(file);
            await _productRepository.AddAsync(product);
            return ProductMapper.toProductResponse(product);
        }

        public async Task<ProductResponse> UpdateProduct(string productId, string? brandId, string? subCategoryId, ProductRequest request, IFormFile? file)
        {
            var product = await _productRepository.GetByIdAsync(productId) ??
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(brandId))
            {
                if (!await _brandRepository.ExistsAsync(brandId))
                {
                    throw new AppException(ErrorCode.BRAND_NOT_EXISTS);
                }
                product.brandId = brandId;
            }
            if (!String.IsNullOrWhiteSpace(subCategoryId))
            {
                if (!await _subCategoryRepository.ExistsAsync(subCategoryId))
                {
                    throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);
                }
                product.subCategoryId = subCategoryId;
            }
            if (!String.IsNullOrWhiteSpace(request.productName))
            {
                product.productName = request.productName;
            }
            if (request.price >= 0)
            {
                product.price = request.price;
            }
            if (request.stockQuantity >= 0)
            {
                product.stockQuantity = request.stockQuantity;
            }
            if (!String.IsNullOrWhiteSpace(request.description))
            {
                product.description = request.description;
            }
            if (file != null)
            {
                if (!string.IsNullOrEmpty(product.thumbnail))
                {
                    await _cloudinaryService.DeleteImageAsync(product.thumbnail);
                }
                product.thumbnail = await _cloudinaryService.UploadImageAsync(file);
            }

            if (!String.IsNullOrWhiteSpace(request.status))
            {
                product.status = request.status;
            }
            await _productRepository.UpdateAsync(product);
            return ProductMapper.toProductResponse(product);
        }

        public async Task<ProductResponse> GetProductById(string id)
        {
            var product = await _productRepository.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            return ProductMapper.toProductResponse(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProduct()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(ProductMapper.toProductResponse);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsBySubCategoryId(string subCategoryId)
        {
            var products = await _productRepository.GetProductsBySubCategoryIdAsync(subCategoryId);
            return products.Select(ProductMapper.toProductResponse);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByBrandId(string brandId)
        {
            var products = await _productRepository.GetProductsByBrandIdAsync(brandId);
            return products.Select(ProductMapper.toProductResponse);
        }

        public async Task<IEnumerable<ProductResponse>> SearchProducts(string searchTerm)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return products.Select(ProductMapper.toProductResponse);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return products.Select(ProductMapper.toProductResponse);
        }
    }
}
