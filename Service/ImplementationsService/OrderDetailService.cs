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
    public class OrderDetailService: IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDetailResponse> CreateOrderDetail(string orderId, string productId, OrderDetailRequest request)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);
            }

            if(! await _productRepository.ExistsAsync(productId))
            {
               throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }
            var orderDetail = OrderDetailMapper.toOrderDetail(request);
            orderDetail.orderId = orderId;
            orderDetail.productId = productId;
            await _orderDetailRepository.AddAsync(orderDetail);
            return OrderDetailMapper.toOrderDetailResponse(orderDetail);
        }

        public async Task<List<OrderDetailResponse>> GetOrderDetailByOrderId(string orderId)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);
            }
            var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
            return orderDetails.Select(od => OrderDetailMapper.toOrderDetailResponse(od)).ToList();
        }

        public async Task<string> DeleteOrderDetailbyOrderId(string orderId)
        {
            if (!await _orderRepository.ExistsAsync(orderId))
            {
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);
            }
           var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
              foreach(var od in orderDetails)
            {
                await _orderDetailRepository.DeleteAsync(od.orderId);
            }
            return "Deleted all order details for orderId: " + orderId;
        }
    }
}
