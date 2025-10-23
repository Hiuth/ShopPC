using PagedList.Core;
using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Exceptions;
using ShopPC.Mapper;
using ShopPC.Models;
using ShopPC.Repository.ImplementationsRepository;
using ShopPC.Repository.InterfaceRepository;
using ShopPC.Service.InterfaceService;


namespace ShopPC.Service.ImplementationsService
{
    public class PcBuildService: IPcBuildService
    {
        private readonly IPcBuildRepository _pcBuildRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IPcBuildItemRepository _pcBuildItemRepository;
        public PcBuildService(IPcBuildRepository pcBuildRepository, ISubCategoryRepository subCategoryRepository, 
            ICloudinaryService cloudinaryService,
            IPcBuildItemRepository pcBuildItemRepository)
        {
            _pcBuildRepository = pcBuildRepository;
            _subCategoryRepository = subCategoryRepository;
            _cloudinaryService = cloudinaryService;
            _pcBuildItemRepository = pcBuildItemRepository;
        }



        private PaginatedResponse<PcBuildResponse> ToPaginatedResponse(IPagedList<PcBuild> pagedList)
        {
            return new PaginatedResponse<PcBuildResponse>
            {
                Items = pagedList.Select(PcBuildMapper.toPcBuildResponse),
                CurrentPage = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.PageCount,
                TotalCount = pagedList.TotalItemCount
            };
        }

        public async Task<PcBuildResponse> CreatePcBuild(string subCategoryId,PcBuildRequest pcBuildRequest, IFormFile file)
        {
            if (!await _subCategoryRepository.ExistsAsync(subCategoryId))
            {
                 throw new AppException(ErrorCode.SUB_CATEGORY_NOT_EXISTS);
            }
            var pcBuild = PcBuildMapper.toPcBuild(pcBuildRequest);
            pcBuild.subCategoryId = subCategoryId;
            pcBuild.thumbnail = await _cloudinaryService.UploadImageAsync(file);
            var createdPcBuild = await _pcBuildRepository.AddAsync(pcBuild);
            return PcBuildMapper.toPcBuildResponse(createdPcBuild);
        }

        public async Task<PcBuildResponse> UpdatePcBuild(string pcBuildId,string? subCategoryId ,PcBuildRequest pcBuildRequest, IFormFile? file)
        {
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildId) ??
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            if (!String.IsNullOrWhiteSpace(pcBuildRequest.productName))
            {
                pcBuild.productName = pcBuildRequest.productName;
            }
            if (pcBuildRequest.price.HasValue)
            {
                pcBuild.price = pcBuildRequest.price.Value;
            }
            if (!String.IsNullOrWhiteSpace(pcBuildRequest.description))
            {
                pcBuild.description = pcBuildRequest.description;
            }

            if (!String.IsNullOrWhiteSpace(pcBuildRequest.status))
            {
                pcBuild.status = pcBuildRequest.status;
            }

            if (file != null)
            {
                if (!String.IsNullOrEmpty(pcBuild.thumbnail))
                {
                    await _cloudinaryService.DeleteImageAsync(pcBuild.thumbnail);
                }
                pcBuild.thumbnail = await _cloudinaryService.UploadImageAsync(file);
            }
            await _pcBuildRepository.UpdateAsync(pcBuild);
            return PcBuildMapper.toPcBuildResponse(pcBuild);
        }

        public async Task<PcBuildResponse> GetPcBuildById(string pcBuildId)
        {
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildId) ??
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            return PcBuildMapper.toPcBuildResponse(pcBuild);
        }

        public async Task<PaginatedResponse<PcBuildResponse>> GetAllPcBuilds(int number, int pagesize)
        {
            var pcBuilds = (await _pcBuildRepository.GetAllPcBuildAsync()).AsQueryable();
            var pageList = new PagedList<PcBuild>(pcBuilds,number, pagesize);
            return ToPaginatedResponse(pageList);
        }

        public async Task<string> DeletePcBuild(string pcBuildId)
        {
            var pcBuild = await _pcBuildRepository.GetPcBuildByIdAsync(pcBuildId) ??
                throw new AppException(ErrorCode.PC_BUILD_NOT_EXISTS);
            if (!String.IsNullOrEmpty(pcBuild.thumbnail))
            {
                await _cloudinaryService.DeleteImageAsync(pcBuild.thumbnail);
            }
            foreach (var item in pcBuild.productImgs)
            {
                if (!String.IsNullOrEmpty(item.imgUrl))
                {
                    await _cloudinaryService.DeleteImageAsync(item.imgUrl);
                }
            }

            foreach(var item in pcBuild.pcBuildItems) {
                await _pcBuildItemRepository.DeleteAsync(item.id);
            }

            await _pcBuildRepository.DeleteAsync(pcBuild.id);

            if(!await _pcBuildRepository.ExistsAsync(pcBuildId))
            {
                return "delete fail";
            }
            return "Deleted pc build with id: " + pcBuildId;
        }

        public async Task<PaginatedResponse<PcBuildResponse>> GetPcBuildsBySubCategoryId(string subCategoryId, int pageNumber,int pageSize)
        { 
            var pcBuilds = (await _pcBuildRepository.GetPcBuildsBySubCategoryIdAsync(subCategoryId)).AsQueryable();
            var pageList = new PagedList<PcBuild>(pcBuilds, pageNumber, pageSize);
            return ToPaginatedResponse(pageList);
        }
    }
}
