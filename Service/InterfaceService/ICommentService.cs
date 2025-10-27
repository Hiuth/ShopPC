using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;

namespace ShopPC.Service.InterfaceService
{
    public interface ICommentService
    {
        Task<CommentResponse> CreateComment(string productId, CommentRequest request);
        Task<List<CommentResponse>> GetCommentsByProductId(string productId);
        Task<List<CommentResponse>> GetCommentsByAccountId();
        Task<string> DeleteComment(string commentId);
        Task<CommentResponse> UpdateComment(string commentId, CommentRequest request);
        Task<RatingSummaryResponse> GetRatingSummaryByProductId(string productId);
    }
}
