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
    public class PcBuildItemService : IPcBuildItemService
    {
        private readonly IPcBuildItemRepository _pcBuildItemRepository;
        private readonly IPcBuildRepository _pcBuildRepository;
        private readonly IProductRepository _productRepository;
        public PcBuildItemService(IPcBuildItemRepository pcBuildItemRepository, IProductRepository productRepository, IPcBuildRepository pcBuildRepository)
        {
            _pcBuildItemRepository = pcBuildItemRepository;
            _productRepository = productRepository;
            _pcBuildRepository = pcBuildRepository;
        }

        public async Task<PcBuildItemResponse> CreatePcBuildItem(string pcBuildId, string productId, PcBuildItemRequest request)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
            }

            if (!await _pcBuildRepository.ExistsAsync(pcBuildId)) 
            { 
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            }
            var pcBuildItem = PcBuildItemMapper.toPcBuildItem(request);
            pcBuildItem.pcBuildId = pcBuildId;
            pcBuildItem.productId = productId;
            await _pcBuildItemRepository.AddAsync(pcBuildItem);

            var product = await _productRepository.GetProductByIdAsync(productId);
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildId);
            var price = request.quantity * product!.price;
            pcBuild!.price += price;
            await _pcBuildRepository.UpdateAsync(pcBuild!);

            return PcBuildItemMapper.toPcBuildItemResponse(pcBuildItem);
        }
        public async Task<PcBuildItemResponse> UpdatePcBuildItem(string id, string? productId, PcBuildItemRequest request)
        {
            var pcBuildItem = await _pcBuildItemRepository.GetPcBuildItemByIdAsync(id) ??
                throw new AppException(ErrorCode.PC_BUILD_ITEM_NOT_EXISTS);
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildItem.pcBuildId)
                ?? throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);

            decimal? oldPrice = pcBuildItem.product.price;
            int oldQuantity = pcBuildItem.quantity;
            decimal? pcPrice = pcBuild.price;
            // Nếu đổi sản phẩm
            if (!string.IsNullOrWhiteSpace(productId))
            {
                if (!await _productRepository.ExistsAsync(productId))
                    throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);

                var newProduct = await _productRepository.GetProductByIdAsync(productId)
                    ?? throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);

                // Trừ đi phần giá cũ của sản phẩm cũ
                pcBuild.price -= oldPrice * oldQuantity;

                pcBuild.price += oldQuantity*newProduct.price;
                // Cập nhật sang sản phẩm mới
                pcBuildItem.productId = productId;
                oldPrice = newProduct.price; // cập nhật giá mới
            }

            // Nếu đổi số lượng
            if (request.quantity != 0)
            {
                if (request.quantity > pcBuildItem.product.stockQuantity)
                    throw new AppException(ErrorCode.INVALID_QUANTITY);

                pcBuild.price = pcBuild.price - (oldQuantity * oldPrice) + (request.quantity * oldPrice);
                pcBuildItem.quantity = request.quantity;
            }

            await _pcBuildItemRepository.UpdateAsync(pcBuildItem);
            await _pcBuildRepository.UpdateAsync(pcBuild);
            return PcBuildItemMapper.toPcBuildItemResponse(pcBuildItem);
        }


        public async Task<string> DeletePcBuildItem(string id)
        {
            var pcBuildItem = await _pcBuildItemRepository.GetPcBuildItemByIdAsync(id) ??
                throw new AppException(ErrorCode.PC_BUILD_ITEM_NOT_EXISTS);
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildItem.pcBuildId);

            pcBuild!.price = pcBuild.price - (pcBuildItem.quantity * pcBuildItem.product.price);

            await _pcBuildItemRepository.DeleteAsync(pcBuildItem.id);
            if (await _pcBuildItemRepository.ExistsAsync(pcBuildItem.id))
            {
                return "Delete pc fail build Item";
            }
            return "Delete pc build Item successfully";
        }

        public async Task<PcBuildItemResponse?> GetPcBuildItemById(string id)
        {
            var pcBuildItem = await _pcBuildItemRepository.GetPcBuildItemByIdAsync(id)??
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            return PcBuildItemMapper.toPcBuildItemResponse(pcBuildItem);
        }

        public async Task<List<PcBuildItemResponse>> GetPcBuildItemsByPcBuildId(string pcBuildId)
        {
            if (!await _pcBuildRepository.ExistsAsync(pcBuildId))
            {
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            }
            var pcBuildItems = await _pcBuildItemRepository.GetPcBuildItemsByPcBuildIdAsync(pcBuildId);
            return pcBuildItems.Select(PcBuildItemMapper.toPcBuildItemResponse).ToList();
        }

       
    }
}
