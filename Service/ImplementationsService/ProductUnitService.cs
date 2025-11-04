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
    public class ProductUnitService : IProductUnitService
    {
        private readonly IProductUnitRepository _productUnitRepository;
        private readonly IProductRepository _productRepository;
        public ProductUnitService(IProductUnitRepository productUnitRepository, IProductRepository productRepository)
        {
            _productUnitRepository = productUnitRepository;
            _productRepository = productRepository;
        }

        public async Task<List<ProductUnitResponse>> GetProductUnitsByProductId(string productId)
        {
            if (!await _productRepository.ExistsAsync(productId)) { 
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
       
            var productUnits = await _productUnitRepository.GetProductUnitsByProductIdAsync(productId);

            return productUnits.Select(ProductUnitMapper.toProductUnitResponse).ToList();
        }

        public async Task<ProductUnitResponse> GetProductUnitById(string productUnitId)
        {
            var productUnit = await _productUnitRepository.GetProductUnitByIdAsync(productUnitId)??
                throw new AppException(ErrorCode.PRODUCT_UNIT_NOT_EXISTS);
            return ProductUnitMapper.toProductUnitResponse(productUnit);
        }

        public async Task<ProductUnitResponse> CreateProductUnit(string productId, ProductUnitRequest request)
        {
            var product = await _productRepository.GetByIdAsync(productId) ??
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            if(!String.IsNullOrWhiteSpace(request.serialNumber))
            {
                if (!await _productUnitRepository.isSerialNumberUniqueAsync(request.serialNumber))
                {
                    throw new AppException(ErrorCode.PRODUCT_UNIT_SERIALNUMBER_ALREADY_EXISTS);
                }
            }

            if (!String.IsNullOrWhiteSpace(request.imei))
            {
                if (!await _productUnitRepository.isImeiUniqueAsync(request.imei))
                {
                    throw new AppException(ErrorCode.PRODUCT_UNIT_IMEI_ALREADY_EXISTS);
                }
            }

           
            var productUnit = ProductUnitMapper.toProductUnit(request);
            productUnit.productId = productId;
            await _productUnitRepository.AddAsync(productUnit);
            product.stockQuantity = await _productUnitRepository.CountProductUnitAvailableByProductIdAsync(productId);
            await _productRepository.UpdateAsync(product);
            return ProductUnitMapper.toProductUnitResponse(productUnit);
        }

        public async Task<ProductUnitResponse> UpdateProductUnit(string productUnitId, ProductUnitRequest request)
        {
            var productUnit = await _productUnitRepository.GetProductUnitByIdAsync(productUnitId) ??
                throw new AppException(ErrorCode.PRODUCT_UNIT_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(request.imei))
            {
                if (!await _productUnitRepository.isImeiUniqueAsync(request.imei))
                {
                    throw new AppException(ErrorCode.PRODUCT_UNIT_IMEI_ALREADY_EXISTS);
                }
                productUnit.imei = request.imei;
            }


            if (!String.IsNullOrWhiteSpace(request.status))
                productUnit.status = request.status;

            if (!String.IsNullOrWhiteSpace(request.serialNumber))
            {
                if (!await _productUnitRepository.isSerialNumberUniqueAsync(request.serialNumber))
                {
                    throw new AppException(ErrorCode.PRODUCT_UNIT_SERIALNUMBER_ALREADY_EXISTS);
                }
                productUnit.serialNumber = request.serialNumber;
            }
           
            await _productUnitRepository.UpdateAsync(productUnit);
            return ProductUnitMapper.toProductUnitResponse(productUnit);
        }

        public async Task<string> DeleteProductUnit(string productUnitId)
        {
            var productUnit = await _productUnitRepository.GetProductUnitByIdAsync(productUnitId) ??
                throw new AppException(ErrorCode.PRODUCT_UNIT_NOT_EXISTS);
            await _productUnitRepository.DeleteAsync(productUnit.id);
            if(await _productUnitRepository.ExistsAsync(productUnitId))
                return "Delete product unit failed";
            return "Delete product unit successfully";
        }
    }
}
