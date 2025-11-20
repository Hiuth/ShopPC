using Microsoft.EntityFrameworkCore;
using ShopPC.Data;
using ShopPC.Models;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class WarrnatyRecordRepository : GenericReopository<WarrantyRecord>, IWarrantyRecordRepository
    {
        public WarrnatyRecordRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WarrantyRecord?> GetWarrantyRecordByIdAsync(string id)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr=>wr.productUnit)
                .FirstOrDefaultAsync(wr => wr.id == id);
        }

        public async Task<WarrantyRecord?> GetWarrantyRecordBySerialNumberAsync(string serialNumber)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .FirstOrDefaultAsync(wr => wr.productUnit.serialNumber == serialNumber);
        }

        public async Task<WarrantyRecord?> GetWarrantyRecordByImeiAsync(string imei)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .FirstOrDefaultAsync(wr => wr.productUnit.imei == imei);
        }

        public async Task<List<WarrantyRecord>> GetWarrantyRecordsByProductIdAsync(string productId)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .Where(wr => wr.productId == productId)
                .ToListAsync();
        }

        public async Task<List<WarrantyRecord>> GetWarrantyRecordsByOrderIdAsync(string orderId)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .Where(wr => wr.orderId == orderId)
                .ToListAsync();
        }

        public async Task<List<WarrantyRecord>> GetWarrantyRecordsByStatusAsync(string status)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .Where(wr => wr.status == status)
                .ToListAsync();
        }
        public async Task<List<WarrantyRecord>> GetWarrantyRecordsByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet
                .Include(wr => wr.product)
                .Include(wr => wr.order)
                .Include(wr => wr.productUnit)
                .Where(wr => wr.order.phoneNumber == phoneNumber)
                .ToListAsync();
        }
    }
}
