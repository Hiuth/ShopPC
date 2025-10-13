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

        public async Task<SubCategoryResponse> createSubCategory(SubCategoryRequest request, IFormFile file)
        { 
            
        }

        public async Task<SubCategoryResponse> updateSubCategory(string id,SubCategoryRequest request, IFormFile file)
        {

        }

        public async Task<List<SubCategoryResponse>> getAllSubCategory()
        {
      
        }

        public async Task<SubCategoryResponse> getSubCategoryById(string id)
        {

        }
        public async Task<List<SubCategoryResponse>> getSubCategoryByCategoryId(string categoryId)
        {
        }
    }
}
