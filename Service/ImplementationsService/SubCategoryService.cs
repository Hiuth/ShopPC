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
    public class SubCategoryService: ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository, ICloudinaryService cloudinaryService)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<SubCategoryResponse> createSubCategory(string categoryId, SubCategoryRequest request, IFormFile file)
        {
            if (await _subCategoryRepository.IsSubCategoryNameUniqueAsync(request.subCategoryName))
            {
                throw new AppException(ErrorCode.SUBCATEGORY_ALREADY_EXISTS);
            }

            var category = await _categoryRepository.GetByIdAsync(categoryId) ??
                throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            var imageUrl = await _cloudinaryService.UploadImageAsync(file);

            var subCategory = SubCategoryMapper.toSubCategory(request);
            subCategory.categoryId = category.id;
            subCategory.subCategoryImg = imageUrl;
            var createdSubCategory = await _subCategoryRepository.AddAsync(subCategory);
            return SubCategoryMapper.toSubCategoryResponse(createdSubCategory);
        }

        public async Task<SubCategoryResponse> updateSubCategory(string subCategoryId,string categoryId,SubCategoryRequest request, IFormFile file)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(subCategoryId) ??
                throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);

            if (!String.IsNullOrWhiteSpace(request.subCategoryName))
            {
                if (await _subCategoryRepository.IsSubCategoryNameUniqueAsync(request.subCategoryName))
                {
                    throw new AppException(ErrorCode.SUBCATEGORY_ALREADY_EXISTS);
                }
                subCategory.subCategoryName = request.subCategoryName;
            }
                 
            if (!string.IsNullOrEmpty(subCategory.subCategoryImg))
            {
               await _cloudinaryService.DeleteImageAsync(subCategory.subCategoryImg);
               subCategory.subCategoryImg = await _cloudinaryService.UploadImageAsync(file);
            }
             
            if (!String.IsNullOrWhiteSpace(request.description))
            {
                subCategory.description = request.description;
            }

            if (!String.IsNullOrWhiteSpace(categoryId))
            {
                if (await _categoryRepository.ExistsAsync(categoryId))
                {
                    subCategory.categoryId = categoryId;
                }
                else
                {
                    throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
                }

            }

            await _subCategoryRepository.UpdateAsync(subCategory);
            return SubCategoryMapper.toSubCategoryResponse(subCategory);
        }

        public async Task<List<SubCategoryResponse>> getAllSubCategory()
        {
           var subCategories = await _subCategoryRepository.GetAllAsync();
           return subCategories.Select(SubCategoryMapper.toSubCategoryResponse).ToList();
        }

        public async Task<SubCategoryResponse> getSubCategoryById(string id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id) ??
                throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);
            return SubCategoryMapper.toSubCategoryResponse(subCategory);
        }
        
        public async Task<List<SubCategoryResponse>> getSubCategoryByCategoryId(string categoryId)
        {
            if (!await _categoryRepository.ExistsAsync(categoryId))
            {
                throw new AppException(ErrorCode.CATEGORY_NOT_EXISTS);
            }
            var subCategories = await _subCategoryRepository.GetSubCategoriesByCategoryIdAsync(categoryId);
            return subCategories.Select(SubCategoryMapper.toSubCategoryResponse).ToList();
        }
    }
}
