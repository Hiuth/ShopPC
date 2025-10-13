using CloudinaryDotNet.Actions;

namespace ShopPC.Service.InterfaceService
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
