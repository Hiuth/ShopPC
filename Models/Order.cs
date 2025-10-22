using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShopPC.Models
{
    public class Order
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime orderDate { get; set; } = DateTime.Now;
        public string status { get; set; } = string.Empty;
        public string accountId { get; set; } = string.Empty;
        public decimal totalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        // 1 order thuộc về 1 account
        [ForeignKey("accountId")]
        public Account account { get; set; } = null!;

        // 1 order có nhiều orderDetail
        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        //1 order có nhiều warrantyRecord
        public ICollection<WarrantyRecord> warrantyRecords { get; set; } = new List<WarrantyRecord>();
    }
}
