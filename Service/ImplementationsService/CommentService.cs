using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;

namespace ShopPC.Service.ImplementationsService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUserService _currentUserService;
        public CommentService(ICommentRepository commentRepository, IProductRepository productRepository,
            IAccountRepository accountRepository, ICurrentUserService currentUserService)
        {
            _commentRepository = commentRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CommentResponse> CreateComment( string productId, CommentRequest request)
        {
            var accountId = _currentUserService.GetCurrentUserId();
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }

            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            if (request.rating < 1 || request.rating > 5)
                throw new AppException(ErrorCode.RATING_INVALID);

            var comment = CommentMapper.toComment(request);
            comment.accountId = accountId;
            comment.productId = productId;
            await _commentRepository.AddAsync(comment);
            return CommentMapper.toCommentResponse(comment);
        }

        public async Task<CommentResponse> UpdateComment(string commentId, CommentRequest request)
        {
            var accountId = _currentUserService.GetCurrentUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId) ??
                throw new AppException(ErrorCode.COMMENT_NOT_EXISTS);
            if (comment.accountId != accountId)
            {
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }

            if (!String.IsNullOrWhiteSpace(request.content))
            {
                comment.content = request.content;
            }
            if (request.rating != 0)
            {
                if (request.rating<1 || request.rating>5)
                {
                    throw new AppException(ErrorCode.RATING_INVALID);
                }
                comment.rating = request.rating;
            }
            await _commentRepository.UpdateAsync(comment);
            return CommentMapper.toCommentResponse(comment);
        }

        public async Task<string> DeleteComment(string commentId)
        {
            var accountId = _currentUserService.GetCurrentUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId) ??
                throw new AppException(ErrorCode.COMMENT_NOT_EXISTS);
            if (comment.accountId != accountId)
            {
                throw new AppException(ErrorCode.UNAUTHORIZED);
            }
             await _commentRepository.DeleteAsync(comment.id);
            if (await _commentRepository.ExistsAsync(commentId))
            {
                return "Delete comment fail";
            }
            return "Delete comment successfully";
        }

        public async Task<List<CommentResponse>> GetCommentsByProductId(string productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            var comments = await _commentRepository.GetCommentByProductIdAsync(productId);
            return comments.Select(CommentMapper.toCommentResponse).ToList();
        }

        public async Task<List<CommentResponse>> GetCommentsByAccountId()
        {
            var accountId = _currentUserService.GetCurrentUserId();
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }
            var comments = await _commentRepository.GetCommentByAccountIdAsync(accountId);
            return comments.Select(CommentMapper.toCommentResponse).ToList();
        }

        public async Task<RatingSummaryResponse> GetRatingSummaryByProductId(string productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);

            var (avg, total, dist) = await _commentRepository.GetRatingSummaryByProductIdAsync(productId);
            return new RatingSummaryResponse
            {
                average = Math.Round(avg, 2),
                total = total,
                star1 = dist[0],
                star2 = dist[1],
                star3 = dist[2],
                star4 = dist[3],
                star5 = dist[4]
            };
        }
    }
}
