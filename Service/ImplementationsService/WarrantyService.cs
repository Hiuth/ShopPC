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
    public class WarrantyService : IWarrantyRecordService
    {
        private readonly IWarrantyRecordRepository _warrantyRecordRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductUnitRepository _productUnitRepository;
        public WarrantyService(IWarrantyRecordRepository warrantyRecordRepository, IProductRepository productRepository, IOrderRepository orderRepository, IProductUnitRepository productUnitRepository)
        {
            _warrantyRecordRepository = warrantyRecordRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _productUnitRepository = productUnitRepository;
        }


        private static DateTime CalculateEndDate(DateTime startDate, int warrantyMonths)
        {
            return startDate.AddMonths(Math.Max(0, warrantyMonths));
        }

        public async Task<WarrantyRecordResponse> CreateWarrantyPeriod(string productId, string orderId, string productUnitId, WarrantyRecordRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(productId) ??
                   throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);

            if (!await _orderRepository.ExistsAsync(orderId))
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);

            if (!await _productUnitRepository.ExistsAsync(productUnitId))
                throw new AppException(ErrorCode.PRODUCT_UNIT_NOT_EXISTS);

            var warranty = WarrantyRecordMapper.toWarrantyRecord(request);
            warranty.productId = productId;
            warranty.orderId = orderId;
            warranty.productUnitId = productUnitId;
            warranty.startDate = DateTime.Now;
            warranty.endDate = CalculateEndDate(warranty.startDate, product.warrantyPeriod ?? 0);
            await _warrantyRecordRepository.AddAsync(warranty);
            return WarrantyRecordMapper.toWarrantyRecordResponse(warranty);
        }

        public async Task<WarrantyRecordResponse> UpdateWarrantyPeriod(string warrantyPeroidId, WarrantyRecordRequest request)
        {
            var warranty = await _warrantyRecordRepository.GetWarrantyRecordByIdAsync(warrantyPeroidId) ??
                throw new AppException(ErrorCode.WARRANTY_RECORD_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(request.status))
                warranty.status = request.status;
            await _warrantyRecordRepository.UpdateAsync(warranty);
            return WarrantyRecordMapper.toWarrantyRecordResponse(warranty);

        }

        public async Task<List<WarrantyRecordResponse>> GetWarrantyRecordByProductId(string productId)
        {
            var warranties = await _warrantyRecordRepository.GetWarrantyRecordsByProductIdAsync(productId);
            return warranties.Select(WarrantyRecordMapper.toWarrantyRecordResponse).ToList();
        }
        public async Task<List<WarrantyRecordResponse>> GetWarrantyRecordByOrderId(string orderId)
        {
            var warranties = await _warrantyRecordRepository.GetWarrantyRecordsByOrderIdAsync(orderId);
            return warranties.Select(WarrantyRecordMapper.toWarrantyRecordResponse).ToList();
        }
        public async Task<List<WarrantyRecordResponse>> GetWarrantyRecordByStatus(string status)
        {
            var warranties = await _warrantyRecordRepository.GetWarrantyRecordsByStatusAsync(status);
            return warranties.Select(WarrantyRecordMapper.toWarrantyRecordResponse).ToList();
        }
        public async Task<WarrantyRecordResponse> GetWarrantyRecordBySerialNumber(string serialNumber)
        {
            var warranty = await _warrantyRecordRepository.GetWarrantyRecordBySerialNumberAsync(serialNumber) ??
                throw new AppException(ErrorCode.WARRANTY_RECORD_NOT_EXISTS);
            return WarrantyRecordMapper.toWarrantyRecordResponse(warranty);
        }
        public async Task<WarrantyRecordResponse> GetWarrnatyRecordByImei(string imei)
        {
            var warranty = await _warrantyRecordRepository.GetWarrantyRecordByImeiAsync(imei) ??
                throw new AppException(ErrorCode.WARRANTY_RECORD_NOT_EXISTS);
            return WarrantyRecordMapper.toWarrantyRecordResponse(warranty);
        }
        public async Task<string> DeleteWarrnatyRecordById(string id)
        {
            var warranty = await _warrantyRecordRepository.GetWarrantyRecordByIdAsync(id) ??
                throw new AppException(ErrorCode.WARRANTY_RECORD_NOT_EXISTS);
            await _warrantyRecordRepository.DeleteAsync(warranty.id);
            return "Warranty record deleted successfully.";
        }
    }
      
 }
