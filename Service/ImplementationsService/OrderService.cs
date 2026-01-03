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
        private readonly EmailService _emailService;
        public OrderService(IOrderRepository orderRepository, IAccountRepository accountRepository,
            IOrderDetailRepository orderDetailRepository, ICurrentUserService currentUserService,
            IProductRepository productRepository, IProductUnitRepository productUnitRepository, EmailService emailService)
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _orderDetailRepository = orderDetailRepository;
            _currentUserService = currentUserService;
            _productRepository = productRepository;
            _productUnitRepository = productUnitRepository;
            _emailService = emailService;
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


        public static string GenerateUserOrderNotification(string status)
        {
            //CONFIRMED,  PROCESSING,SHIPPED,DELIVERED,  CANCELED
            string statusText = status switch
            {
                "CONFIRMED" => "xác nhận",
                "PROCESSING" => "đang xử lý",
                "SHIPPED" => "đã bàn giao cho đơn vị vận chuyển",
                "DELIVERED" => "giao hàng thành công",
                "CANCELED" => "đã bị hủy",
                _ => "cập nhật trạng thái"
            };

            return $@"
                <div style='font-family: Arial, Helvetica, sans-serif; background-color: #eef4ff; padding: 24px;'>
                    <div style='max-width: 620px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 28px; border-top: 6px solid #2d89ef; box-shadow: 0 4px 12px rgba(45,137,239,0.15);'>
        
                        <h2 style='color: #2d89ef; margin: 0 0 16px 0; font-weight: 600;'>
                            Thông báo từ Nexora
                        </h2>

                        <p style='font-size: 17px; color: #1f2d3d; margin: 12px 0;'>
                            <b>Đơn hàng của bạn đã được {statusText}.</b>
                        </p>

                        <p style='font-size: 14.5px; color: #555555; line-height: 1.7; margin: 8px 0;'>
                            Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của Nexora.
                            Chúng tôi sẽ tiếp tục cập nhật trạng thái đơn hàng trong thời gian sớm nhất.
                        </p>

                        <p style='font-size: 14.5px; color: #555555; line-height: 1.7; margin: 8px 0;'>
                            Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ bộ phận hỗ trợ của chúng tôi để được hỗ trợ kịp thời.
                        </p>

                        <div style='margin-top: 22px; padding-top: 14px; border-top: 1px solid #e6eaf0;'>
                            <p style='font-size: 12.5px; color: #8a8a8a; text-align: center; margin: 0;'>
                                © Nexora – Hệ thống thương mại điện tử
                            </p>
                        </div>

                    </div>
                </div>";
        }




        public async Task<OrderResponse> UpdateOrder(string orderId, OrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) ??
                throw new AppException(ErrorCode.ORDER_NOT_EXISTS);

            var accountId = await _orderRepository.GetAccountIdByOrderIdAsync(orderId)??
                throw new AppException(ErrorCode.ACCOUNT_NOT_EXISTS);

            var user = await _accountRepository.GetAccountById(accountId);
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

                _=Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendEmailAsync(user.email, "Cập nhật trạng thái đơn hàng", GenerateUserOrderNotification(order.status));
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nếu cần thiết
                        Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                    }
                });
                
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
