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
    public class ProductImgService: IProductImgService
    {
        private readonly IProductImgRepository _productImgRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductImgService(IProductImgRepository productImgRepository, IProductRepository productRepository, ICloudinaryService cloudinaryService)
        {
            _productImgRepository = productImgRepository;
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ProductImgResponse> CreateProductImg(string productId, IFormFile file)
        {
            if(!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }

            var request = new ProductImgRequest
            {
                imgUrl = await _cloudinaryService.UploadImageAsync(file)
            };
            var productImg = ProductImgMapper.toProductImg(request);
            productImg.productId = productId;
            return ProductImgMapper.toProductImgResponse(await _productImgRepository.AddAsync(productImg));
        }

        public async Task<List<ProductImgResponse>> GetProductImgByProductId(string productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            var productImg = await _productImgRepository.GetImagesByProductIdAsync(productId);
            return productImg.Select(ProductImgMapper.toProductImgResponse).ToList();
        }

        public async Task<string> DeleteProductImg(string productImgId)
        {
            ProductImg productImg = await _productImgRepository.GetByIdAsync(productImgId) ??
                throw new AppException(ErrorCode.PRODUCT_IMG_NOT_EXISTS);
            await _cloudinaryService.DeleteImageAsync(productImg.imgUrl);
            await _productImgRepository.DeleteAsync(productImg.productId);
            if(await _productImgRepository.ExistsAsync(productImgId))
            {
                return "Delete product img fail";
            }
            else
            {
                return "Delete product img successfully";
            }
        }
    }
}
