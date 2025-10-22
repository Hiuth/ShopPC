using System.Linq.Expressions;
using ShopPC.Models;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IWarrantyRecordRepository: IGenericRepository<WarrantyRecord>
    {
        Task<WarrantyRecord?> GetWarrantyRecordByIdAsync(string id);

        Task<WarrantyRecord?> GetWarrantyRecordBySerialNumberAsync(string serialNumber);

        Task<WarrantyRecord?> GetWarrantyRecordByImeiAsync(string imei);

        Task<List<WarrantyRecord>> GetWarrantyRecordsByProductIdAsync(string productId);

        Task<List<WarrantyRecord>> GetWarrantyRecordsByOrderIdAsync(string orderId);

        Task<List<WarrantyRecord>> GetWarrantyRecordsByStatusAsync(string status);

        //Task<bool> IsWarrantyActiveAsync(string productId, string orderId);
    }
}
