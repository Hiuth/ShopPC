using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class CommentMapper
    {
        public static Comment toComment(CommentRequest request)
        {
            return new Comment
            {
                content = request.content,
                rating = request.rating
            };
        }

        public static CommentResponse toCommentResponse(Comment comment)
        {
            return new CommentResponse
            {
                id = comment.id,
                productId = comment.productId,
                accountId = comment.accountId,
                accountName = comment.account.userName ?? string.Empty,
                content = comment.content,
                rating = comment.rating,
                createdAt = comment.createdAt
            };
        }
    }
}
