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
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductRepository _productRepository;
        private readonly IProductUnitRepository _productUnitRepository;
        public OrderService(IOrderRepository orderRepository, IAccountRepository accountRepository,
            IOrderDetailRepository orderDetailRepository, ICurrentUserService currentUserService,
            IProductRepository productRepository, IProductUnitRepository productUnitRepository)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _orderDetailRepository = orderDetailRepository;
            _currentUserService = currentUserService;
            _productRepository = productRepository;
            _productUnitRepository = productUnitRepository;
        }

        public async Task<OrderResponse> CreateOrder(OrderRequest request)
        {
            var accountId = _currentUserService.GetCurrentUserId();
            Account? account = null;

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                account = await _accountRepository.GetAccountById(accountId);
                if (account == null)
                    throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);

                if (account.roleName == "ADMIN")
                    accountId = null;
            }

            var order = OrderMapper.toOrder(request);
            order.accountId = accountId;

            await _orderRepository.AddAsync(order);
            return OrderMapper.toOrderResponse(order);
        }

        public async Task<OrderResponse> UpdateOrder(string orderId, OrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);

            // Nếu trạng thái thay đổi
            if (!String.IsNullOrWhiteSpace(request.status))
            {
                // Nếu đơn hàng được xác nhận -> trừ tồn kho
                if (request.status == "CONFIRMED" && order.status != "CONFIRMED")
                {
                    var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);

                    foreach (var detail in orderDetails)
                    {
                        var product = await _productRepository.GetProductByIdAsync(detail.productId);
                        if (product == null)
                            throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);

                        // Nếu sản phẩm có serial
                        if (product.isSerial == true)
                        {
                            var availableUnits = await _productUnitRepository
                                .GetAvailableUnitsByProductId(detail.productId, detail.quantity);

                            if (availableUnits.Count < detail.quantity)
                                throw new AppException(ErrorCode.NOT_ENOUGH_UNIT);

                            // Đánh dấu các unit là đã bán
                            foreach (var unit in availableUnits)
                            {
                                unit.status = "SOLD";
                                await _productUnitRepository.UpdateAsync(unit);
                            }

                            // Cập nhật lại tồn kho
                            var remaining = await _productUnitRepository.CountProductUnitAvailableByProductIdAsync(detail.productId);
                            product.stockQuantity = remaining;
                            await _productRepository.UpdateAsync(product);
                        }
                        else
                        {
                            // Nếu sản phẩm không có serial → trừ trực tiếp số lượng tồn
                            if (product.stockQuantity < detail.quantity)
                                throw new AppException(ErrorCode.NOT_ENOUGH_STOCK);

                            product.stockQuantity -= detail.quantity;
                            await _productRepository.UpdateAsync(product);
                        }
                    }
                }

                order.status = request.status;
            }

            // Cập nhật các thông tin khác
            if (!String.IsNullOrWhiteSpace(request.phoneNumber))
            {
                order.phoneNumber = request.phoneNumber;
            }
            if (!String.IsNullOrWhiteSpace(request.customerName))
            {
                order.CustomerName = request.customerName;
            }
            if (!String.IsNullOrWhiteSpace(request.address))
            {
                order.address = request.address;
            }

            if (request.isPaid.HasValue)
            {
                order.isPaid = request.isPaid.Value;
            }

            await _orderRepository.UpdateAsync(order);
            return OrderMapper.toOrderResponse(order);
        }


        public async Task<List<OrderResponse>> GetOrdersByAccountId()
        {
            var accountId = _currentUserService.GetCurrentUserId();
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
            }
            var orders = await _orderRepository.GetOrdersByUserIdAsync(accountId);
            return orders.Select(OrderMapper.toOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(OrderMapper.toOrderResponse).ToList();
        }

        public async Task<string> DeleteOrder(string orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);
            var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
            foreach (var od in orderDetails)
            {
                await _orderDetailRepository.DeleteAsync(od.orderId);
            }
            await _orderRepository.DeleteAsync(orderId);
            return "Deleted order with id: " + orderId;
        }
    }
}
