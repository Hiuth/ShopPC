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
    public class CommentSerivce : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        public CommentSerivce(ICommentRepository commentRepository, IProductRepository productRepository, IAccountRepository accountRepository)
        {
            _commentRepository = commentRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
        }

        public async Task<CommentResponse> createCategory(string accountId, string productId, CommentRequest request)
        {
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }

            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }

            var comment = CommentMapper.toComment(request);
            comment.accountId = accountId;
            comment.productId = productId;
            await _commentRepository.AddAsync(comment);
            return CommentMapper.toCommentResponse(comment);
        }
    }
}
