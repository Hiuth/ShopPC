using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;
using System.Threading.Tasks;
using PagedList.Core;


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

        private PaginatedResponse<ProductResponse> ToPaginatedResponse(IPagedList<Products> pagedList)
        {
            return new PaginatedResponse<ProductResponse>
            {
                Items = pagedList.Select(ProductMapper.toProductResponse),
                CurrentPage = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.PageCount,
                TotalCount = pagedList.TotalItemCount
            };
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
            var product = await _productRepository.GetProductByIdAsync(productId) ??
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
            if (!String.IsNullOrWhiteSpace(request.warrantyPeriod))
            {
                product.warrantyPeriod = request.warrantyPeriod;
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



        public async Task<PaginatedResponse<ProductResponse>> GetAllProduct(int pageNumber, int pageSize)
        {
            var allProducts = (await _productRepository.GetAllProductAsync()).AsQueryable();
            var pagedList = new PagedList<Products>(allProducts, pageNumber, pageSize);
            return ToPaginatedResponse(pagedList);
        }

        public async Task<PaginatedResponse<ProductResponse>> GetProductsBySubCategoryId(string subCategoryId, int pageNumber, int pageSize)
        {
            var products = (await _productRepository.GetProductsBySubCategoryIdAsync(subCategoryId)).AsQueryable();
            var pagedList = new PagedList<Products>(products, pageNumber, pageSize);
            return ToPaginatedResponse(pagedList);
        }

        public async Task<PaginatedResponse<ProductResponse>> GetProductsByBrandId(string brandId, int pageNumber, int pageSize)
        {
            var products = (await _productRepository.GetProductsByBrandIdAsync(brandId)).AsQueryable();
            var pagedList = new PagedList<Products>(products, pageNumber, pageSize);
            return ToPaginatedResponse(pagedList);
        }

        public async Task<PaginatedResponse<ProductResponse>> SearchProducts(string searchTerm, int pageNumber, int pageSize)
        {
            var products = (await _productRepository.SearchProductsAsync(searchTerm)).AsQueryable();
            var pagedList = new PagedList<Products>(products, pageNumber, pageSize);
            return ToPaginatedResponse(pagedList);
        }

        public async Task<PaginatedResponse<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, int pageNumber, int pageSize)
        {
            var products = (await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice)).AsQueryable();
            var pagedList = new PagedList<Products>(products, pageNumber, pageSize);
            return ToPaginatedResponse(pagedList);
        }
    }
}
