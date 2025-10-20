using System.ComponentModel.DataAnnotations;

namespace ShopPC.DTO.Request
{
    public class ProductUnitRequest
    {
        public string? imei { get; set; }
        public string? serialNumber { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
