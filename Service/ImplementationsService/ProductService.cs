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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPcBuildRepository _pcBuildRepository;
        private readonly IPcBuildItemRepository _pcBuildItemRepository;
        public ProductService(IProductRepository productRepository, IBrandRepository brandRepository,
            ISubCategoryRepository subCategoryRepository, ICloudinaryService cloudinaryService, 
            ICategoryRepository categoryRepository, IPcBuildRepository pcBuildRepository,
            IPcBuildItemRepository pcBuildItemRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _subCategoryRepository = subCategoryRepository;
            _cloudinaryService = cloudinaryService;
            _categoryRepository = categoryRepository;
            _pcBuildRepository = pcBuildRepository;
            _pcBuildItemRepository = pcBuildItemRepository;
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


        public async Task<ProductResponse> CreateProduct(string brandId, string categoryId, string? subCategoryId, ProductRequest request, IFormFile file)
        {
            if (!await _brandRepository.ExistsAsync(brandId))
            {
                throw new AppException(ErrorCode.BRAND_NOT_EXISTS);
            }

            if(!await _categoryRepository.ExistsAsync(categoryId))
            {
                    throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }

            var product = ProductMapper.toProducts(request);

            if (!String.IsNullOrWhiteSpace(subCategoryId))
            {
                if (!await _subCategoryRepository.ExistsAsync(subCategoryId))
                {
                    throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);
                }
                product.subCategoryId = subCategoryId;
            }

            product.brandId = brandId;
            product.categoryId = categoryId;
            product.thumbnail = await _cloudinaryService.UploadImageAsync(file);
            await _productRepository.AddAsync(product);
            return ProductMapper.toProductResponse(product);
        }

        public async Task<ProductResponse> UpdateProduct(string productId,string? categoryId ,string? brandId, string? subCategoryId, ProductRequest request, IFormFile? file)
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

            if (!String.IsNullOrWhiteSpace(categoryId))
            {
                if (!await _categoryRepository.ExistsAsync(categoryId))
                {
                    throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
                }
                product.categoryId = categoryId;
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

            if (request.price.HasValue)
            {
                decimal? oldPrice = product.price;
                decimal newPrice = request.price.Value;

                // Cập nhật giá sản phẩm
                product.price = newPrice;

                // Nếu giá có thay đổi, cập nhật các pcBuild chứa sản phẩm này
                if (newPrice != oldPrice)
                {
                    var pcBuildItems = await _pcBuildItemRepository.GetPcBuildItemByProductIdAsync(productId);

                    foreach (var item in pcBuildItems)
                    {
                        var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(item.pcBuildId);
                        if (pcBuild != null)
                        {
                            pcBuild.price = pcBuild.price - (oldPrice * item.quantity) + (newPrice * item.quantity);
                            await _pcBuildRepository.UpdateAsync(pcBuild);
                        }
                    }
                }
            }
            if (request.stockQuantity.HasValue)
            {
                product.stockQuantity = request.stockQuantity.Value;
            }
            if (!String.IsNullOrWhiteSpace(request.description))
            {
                product.description = request.description;
            }
            if (request.warrantyPeriod.HasValue)
            {
                product.warrantyPeriod = request.warrantyPeriod.Value;
            }

            if (file != null)
            {
                if (!String.IsNullOrEmpty(product.thumbnail))
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
            var product = await _productRepository.GetProductByIdAsync(id) ??
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

        public async Task<PaginatedResponse<ProductResponse>> GetProductByCategoryId(string categoryId, int pageNumber, int pageSize)
        {
            var products = (await _productRepository.GetProductsByCategoryIdAsync(categoryId)).AsQueryable();
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
