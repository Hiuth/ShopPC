using ShopPC.Repository.InterfaceRepository;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;
using ShopPC.Service.InterfaceService;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Exceptions;
using ShopPC.Mapper;
using System.Threading.Tasks;
using PagedList.Core;


namespace ShopPC.Service.ImplementationsService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUserService _currentUserService;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, 
            IAccountRepository accountRepository, ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CartResponse> AddToCart(string productId, CartRequest request)
        {
            var accountId = _currentUserService.GetCurrentUserId()
                ?? throw new AppException(ErrorCode.NOT_AUTHENTICATED);

            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            if(request.quantity > product.stockQuantity) { 
                throw new AppException(ErrorCode.QUANTITY_EXCEEDS_STOCK);
            }

            if (await _cartRepository.IsProductInCartAsync(accountId,productId))
            {
                var newCart = await _cartRepository.GetCartByProductIdAndProductIdAsync(accountId, productId)
                    ?? throw new AppException(ErrorCode.CART_NOT_EXISTS);
                if((newCart.quantity + request.quantity) > product.stockQuantity)
                {
                    throw new AppException(ErrorCode.QUANTITY_EXCEEDS_STOCK);
                }
                newCart.quantity += request.quantity;
                await _cartRepository.UpdateAsync(newCart);
                return CartMapper.toCartResponse(newCart);
            }    

            var cart = CartMapper.toCart(request);
            cart.accountId = accountId;
            cart.productId = productId;
            await _cartRepository.AddAsync(cart);
            return CartMapper.toCartResponse(cart);
        }

        public async Task<CartResponse> UpdateCart(string cartId, CartRequest request)
        {
            var cart = await _cartRepository.GetCartByCartIdAsync(cartId)
                ?? throw new AppException(ErrorCode.CART_NOT_EXISTS);

            if (request.quantity > 0)
            {
                if (request.quantity > cart.product.stockQuantity)
                {
                    throw new AppException(ErrorCode.QUANTITY_EXCEEDS_STOCK);
                }
                cart.quantity = request.quantity;
            }
            await _cartRepository.UpdateAsync(cart);
            return CartMapper.toCartResponse(cart);
        }

        public async Task<List<CartResponse>> GetCartByAccountId()
        {
            var accountId = _currentUserService.GetCurrentUserId()
                ?? throw new AppException(ErrorCode.NOT_AUTHENTICATED);
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }
            var carts = await _cartRepository.GetCartByAccountIdAsync(accountId);
            return carts.Select(CartMapper.toCartResponse).ToList();
        }

        public async Task<string> ClearCart(string cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                ?? throw new AppException(ErrorCode.CART_NOT_EXISTS);
            await _cartRepository.DeleteAsync(cart.id);
            return "Delete cart successfully";
        }

        public async Task<string> ClearAllCart()
        {
            var accountId = _currentUserService.GetCurrentUserId()
                ?? throw new AppException(ErrorCode.NOT_AUTHENTICATED);
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }
            var carts = await _cartRepository.GetCartByAccountIdAsync(accountId);
            foreach (var cart in carts)
            {
                await _cartRepository.DeleteAsync(cart.id);
            }
            return "Delete all cart successfully";

        }
    }
}
