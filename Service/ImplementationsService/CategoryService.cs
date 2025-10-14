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
        private readonly ICloudinaryService _cloudinaryService;

        // Inject interface repository thay vì class cụ thể
        public CategoryService(ICategoryRepository categoryRepository, ICloudinaryService cloudinaryService)
        {
            _categoryRepository = categoryRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<CategoryResponse> createCategory(CategoryRequest request, IFormFile file)
        {
            if (await _categoryRepository.IsCategoryNameUniqueAsync(request.categoryName))
            {
                throw new AppException(ErrorCode.CATEGORY_ALREADY_EXISTS);
            }
            var imageUrl = await _cloudinaryService.UploadImageAsync(file);
            request.iconImg = imageUrl;
            var category = CategoryMapper.toCategory(request);
            var createdCategory = await _categoryRepository.AddAsync(category);
            return CategoryMapper.toCategoryResponse(createdCategory);
        }

        public async Task<CategoryResponse> updateCategory(string id, CategoryRequest request, IFormFile? file)
        {
            Category category = await _categoryRepository.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(request.categoryName))
            {
                category.categoryName = request.categoryName;
            }

            if (file != null)
            {
                if (!string.IsNullOrEmpty(category.iconImg))
                {
                    await _cloudinaryService.DeleteImageAsync(category.iconImg);
                }
                category.iconImg = await _cloudinaryService.UploadImageAsync(file);
            }
            await _categoryRepository.UpdateAsync(category);
            return CategoryMapper.toCategoryResponse(category);
        }

        public async Task<List<CategoryResponse>> getAllCategory()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(CategoryMapper.toCategoryResponse).ToList();
        }

        public async Task<CategoryResponse> getCategoryById(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.CATEGORY_NOT_FOUND);
            return CategoryMapper.toCategoryResponse(category);
        }
    }
}
