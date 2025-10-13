using CloudinaryDotNet.Actions;

namespace ShopPC.Service.InterfaceService
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
