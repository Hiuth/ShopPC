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

        }

        public async Task<ProductAttributeResponse> UpdateProductAttribute(string productAttributeId,string? attributeId,ProductAttributeRequest request)
        {

        }

        public async Task<List<ProductAttributeResponse>> GetProductAttributeByProductId(string productId)
        {

        }

        public async Task<string> DeleteProductAttribute(string productAttributeId)
        {

        }
    }
}
