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
    public class AttributesService: IAttributesService
    {
        private readonly IAttributesRepository _attributeRepository;
        private readonly ICategoryRepository _categoryRepository;
        public AttributesService(IAttributesRepository attributeRepository, ICategoryRepository categoryRepository)
        {
            _attributeRepository = attributeRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<AttributesResponse> CreateAttribute(string categoryId, AttributesRequest request)
        {
            if (!await _categoryRepository.ExistsAsync(categoryId))
            {
                throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }
            var attribute = AttributesMapper.toAttributes(request);
            attribute.categoryId = categoryId;
            var createdAttribute = await _attributeRepository.AddAsync(attribute);
            return AttributesMapper.toAttributesResponse(createdAttribute);
        }


        public async Task<AttributesResponse> UpdateAttribute(string attributeId, string? categoryId, AttributesRequest request)
        {
            var attribute = await _attributeRepository.GetByIdAsync(attributeId) ??
                throw new AppException(ErrorCode.ATTRIBUTE_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(categoryId))
            {
                if (!await _categoryRepository.ExistsAsync(categoryId))
                {
                    throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
                }
                attribute.categoryId = categoryId;
            }
            if (!String.IsNullOrWhiteSpace(request.attributeName))
            {
                attribute.attributeName = request.attributeName;
            }
             await _attributeRepository.UpdateAsync(attribute);
            return AttributesMapper.toAttributesResponse(attribute);
        }

        public async Task<AttributesResponse> GetAttributeById(string id)
        {
            var attribute = await _attributeRepository.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.ATTRIBUTE_NOT_EXISTS);
            return AttributesMapper.toAttributesResponse(attribute);
        }

        public async Task<List<AttributesResponse>> GetAllAttributes()
        {
            var attributes = await _attributeRepository.GetAllAsync();
            return attributes.Select(AttributesMapper.toAttributesResponse).ToList();
        }

        public async Task<List<AttributesResponse>> GetAttributesByCategoryId(string categoryId)
        {
            if (!await _categoryRepository.ExistsAsync(categoryId))
            {
                throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }
            var attributes = await _attributeRepository.GetAttributesByCategoryId(categoryId);
            return attributes.Select(AttributesMapper.toAttributesResponse).ToList();
        }
    }
}
