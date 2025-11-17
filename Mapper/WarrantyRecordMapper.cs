using ShopPC.Models;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;

namespace ShopPC.Mapper
{
    public class WarrantyRecordMapper
    {
        public static WarrantyRecord toWarrantyRecord(WarrantyRecordRequest request)
        {
            return new WarrantyRecord
            {
                status = request.status
            };
        }

        public static WarrantyRecordResponse toWarrantyRecordResponse(WarrantyRecord request)
        {
            return new WarrantyRecordResponse
            {
                id = request.id,
                startDate = request.startDate,
                endDate = request.endDate,
                status = request.status,
                productId = request.productId,
                productName = request.product.productName,
                warrantyPeriod = request.product.warrantyPeriod ??0,
                orderId = request.orderId,
                productUnitId = request.productUnitId??string.Empty,
                serialNumber = request.productUnit?.serialNumber??string.Empty,
                imei = request.productUnit?.imei ?? string.Empty
            };
        }
    }
}
