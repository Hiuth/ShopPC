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
    public class ProductAttributeService : IProductAttributeSerivce
    {
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IProductRepository _productRepository;

        public ProductAttributeService(IProductAttributeRepository productAttributeRepository, IAttributesRepository attributesRepository, IProductRepository productRepository)
        {
            _productAttributeRepository = productAttributeRepository;
            _attributesRepository = attributesRepository;
            _productRepository = productRepository;
        }

        public async Task<ProductAttributeResponse> CreateProductAttribute(string attributeId, string productId, ProductAttributeRequest request)
        {
            if (!await _attributesRepository.ExistsAsync(attributeId))
            {
                throw new AppException(ErrorCode.ATTRIBUTE_NOT_EXISTS);
            }
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            var productAttribute = ProductAttributeMapper.toProductAttribute(request);
            productAttribute.attributeId = attributeId;
            productAttribute.productId = productId;
            await _productAttributeRepository.AddAsync(productAttribute);
            return ProductAttributeMapper.toProductAttributeResponse(productAttribute);
        }

        public async Task<ProductAttributeResponse> UpdateProductAttribute(string productAttributeId,string? attributeId,ProductAttributeRequest request)
        {
            if (!await _productAttributeRepository.ExistsAsync(productAttributeId))
            {
                throw new AppException(ErrorCode.PRODUCT_ATTRIBUTE_NOT_EXISTS);
            }
            var productAttribute = await _productAttributeRepository.GetByIdAsync(productAttributeId)
                ?? throw new AppException(ErrorCode.PRODUCT_ATTRIBUTE_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(attributeId))
            {
                if (!await _attributesRepository.ExistsAsync(attributeId))
                {
                    throw new AppException(ErrorCode.ATTRIBUTE_NOT_EXISTS);
                }
                productAttribute.attributeId = attributeId;
            }

            if (!String.IsNullOrWhiteSpace(request.value))
            {
                productAttribute.value = request.value;
            }

            await _productAttributeRepository.UpdateAsync(productAttribute);
            return ProductAttributeMapper.toProductAttributeResponse(productAttribute);
        }

        public async Task<List<ProductAttributeResponse>> GetProductAttributeByProductId(string productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            var productAttributes = await _productAttributeRepository.GetProductAttributesByProductIdAsync(productId);
            return productAttributes.Select(ProductAttributeMapper.toProductAttributeResponse).ToList();
        }

        public async Task<string> DeleteProductAttribute(string productAttributeId)
        {
            var productAttribute = await _productAttributeRepository.GetByIdAsync(productAttributeId) ??
                throw new AppException(ErrorCode.PRODUCT_ATTRIBUTE_NOT_EXISTS);
            await _productAttributeRepository.DeleteAsync(productAttributeId);
            if (await _productAttributeRepository.ExistsAsync(productAttributeId))
            {
                return "Delete product attribute failed";
            }
            return "Delete product attribute successfully";
        }
    }
}
