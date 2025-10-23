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
            return PcBuildItemMapper.toPcBuildItemResponse(pcBuildItem);
        }
        public async Task<PcBuildItemResponse> UpdatePcBuildItem(string id, string? productId, PcBuildItemRequest request)
        {
            var pcBuildItem = await _pcBuildItemRepository.GetPcBuildItemByIdAsync(id) ??
                throw new AppException(ErrorCode.PC_BUILD_ITEM_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(productId))
            {
                if (!await _productRepository.ExistsAsync(productId))
                {
                    throw new AppException(ErrorCode.PRODUCT_NOT_EXISTS);
                }
                pcBuildItem.productId = productId;
            }

            if (request.quantity != 0)
            {
                if (request.quantity > pcBuildItem.product.stockQuantity)
                {
                    throw new AppException(ErrorCode.INVALID_QUANTITY);
                }
                pcBuildItem.quantity = request.quantity;
            }

            await _pcBuildItemRepository.UpdateAsync(pcBuildItem);
            return PcBuildItemMapper.toPcBuildItemResponse(pcBuildItem);
        }

        public async Task<string> DeletePcBuildItem(string id)
        {
            var pcBuildItem = await _pcBuildItemRepository.GetPcBuildItemByIdAsync(id) ??
                throw new AppException(ErrorCode.PC_BUILD_ITEM_NOT_EXISTS);
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
