using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Service.InterfaceService
{
    public interface IWarrantyRecordService
    {
        Task<WarrantyRecordResponse> CreateWarrantyPeriod(string productId, string orderId, string productUnitId, WarrantyRecordRequest request);

        Task<WarrantyRecordResponse> UpdateWarrantyPeriod(string warrantyPeroidId, WarrantyRecordRequest request);

        Task<List<WarrantyRecordResponse>> GetWarrantyRecordByProductId(string productId);

        Task<List<WarrantyRecordResponse>> GetWarrantyRecordByOrderId(string orderId);

        Task<List<WarrantyRecordResponse>> GetWarrantyRecordByStatus(string status);

        Task<WarrantyRecordResponse> GetWarrantyRecordBySerialNumber(string serialNumber);

        Task<WarrantyRecordResponse> GetWarrnatyRecordByImei(string imei);

        Task<List<WarrantyRecordResponse>> GetWarrantyRecordsByPhoneNumber(string phoneNumber);

        Task<string> DeleteWarrnatyRecordById(string id);
    }
}
