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
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        // Inject interface repository thay vì class cụ thể
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> createCategory(CategoryRequest request)
        {
            if (await _categoryRepository.IsCategoryNameUniqueAsync(request.categoryName))
            {
                throw new AppException(ErrorCode.CATEGORY_ALREADY_EXISTS);
            }
            var category = CategoryMapper.toCategory(request);
            var createdCategory = await _categoryRepository.AddAsync(category);
            return CategoryMapper.toCategoryResponse(createdCategory);
        }

        
    }
}
