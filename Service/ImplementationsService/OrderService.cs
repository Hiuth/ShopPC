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
        public OrderService(IOrderRepository orderRepository, IAccountRepository accountRepository,
            IOrderDetailRepository orderDetailRepository, ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _orderDetailRepository = orderDetailRepository;
            _currentUserService = currentUserService;
        }

        public async Task<OrderResponse> CreateOrder( OrderRequest request)
        {
            var accountId = _currentUserService.GetCurrentUserId();
            if (!await _accountRepository.ExistsAsync(accountId))
            {
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);
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

            if (!String.IsNullOrWhiteSpace(request.status))
            {
                order.status = request.status;
            }
            if (!String.IsNullOrWhiteSpace(request.phoneNumber))
            {
                order.phoneNumber = request.phoneNumber;
            }
            if(!String.IsNullOrWhiteSpace(request.customerName))
            {
                order.CustomerName = request.customerName;
            }
            if(!String.IsNullOrWhiteSpace(request.address))
            {
                order.address = request.address;
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
