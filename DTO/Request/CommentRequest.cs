using System.ComponentModel.DataAnnotations;

namespace ShopPC.DTO.Request
{
    public class CommentRequest
    {
        public string content { get; set; } = string.Empty;
        [Range(1, 5)]
        public int rating { get; set; }
    }
}
