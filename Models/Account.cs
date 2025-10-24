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
        public string accountImg { get; set; } = string.Empty;
        // 1 account có nhiều role
        //public ICollection<Role> roles { get; set; } = new List<Role>();

        [ForeignKey(nameof(Role))]
        public string roleName { get; set; } = "USER";
        public Role Role { get; set; } = null!;

        // 1 account có nhiều order
        public ICollection<Order> orders { get; set; } = new List<Order>();

        //1 account có nhiều cart
        public ICollection<Cart> carts { get; set; } = new List<Cart>();

        //1 account có nhiều notification
        public ICollection<Notification> notifications { get; set; } = new List<Notification>();

        //1 account có nhiều comment
        public ICollection<Comment> comments { get; set; } = new List<Comment>();

        //1 account có nhiều report
        public ICollection<Report> reports { get; set; } = new List<Report>();
    }
}
