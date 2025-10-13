using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShopPC.Models
{
    [Index(nameof(email), IsUnique = true)]
    public class Account
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.Now;
        public string gender { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        // 1 account có nhiều role
        //public ICollection<Role> roles { get; set; } = new List<Role>();

        // 1 account có nhiều order
        public ICollection<Order> orders { get; set; } = new List<Order>();

        //1 account có nhiều cart
        public ICollection<Cart> carts { get; set; } = new List<Cart>();

        //1 account có nhiều notification
        public ICollection<Notification> notifications { get; set; } = new List<Notification>();
    }
}
