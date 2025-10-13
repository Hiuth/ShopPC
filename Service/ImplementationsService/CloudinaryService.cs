using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using ShopPC.Configuration;
using ShopPC.Service.InterfaceService;
using Microsoft.Extensions.Logging;
using ShopPC.Exceptions;

namespace ShopPC.Service.ImplementationsService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private const long MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB

        public CloudinaryService(IOptions<CloudinaryConfig> config, ILogger<CloudinaryService> logger)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            ValidateFile(file);

            try
            {
                string originalFilename = file.FileName;
                string fileExtension = "";

                // Get extension (like .jpg, .png)
                if (!string.IsNullOrEmpty(originalFilename) && originalFilename.Contains("."))
                {
                    fileExtension = Path.GetExtension(originalFilename);
                }

                // Generate a clean filename without any extensions
                string fileName = Guid.NewGuid().ToString();

                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, stream),
                    PublicId = fileName,
                    Folder = "shoppc",
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    _logger.LogError("Upload failed: {ErrorMessage}", uploadResult.Error.Message);
                    throw new InvalidOperationException($"Upload failed: {uploadResult.Error.Message}");
                }

                string url = uploadResult.SecureUrl.ToString();
                _logger.LogInformation("File uploaded successfully to Cloudinary: {Url}", url);
                return url;
            }
            catch (Exception e)
            {
                _logger.LogError("Upload failed: {ErrorMessage}", e.Message);
                throw new InvalidOperationException("File upload failed", e);
            }
        }

        public async Task<DeletionResult> DeleteImageAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                return new DeletionResult();
            }

            try
            {
                // Lấy public_id từ URL Cloudinary
                string publicId = ExtractPublicId(fileUrl);
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                _logger.LogInformation("File deleted successfully from Cloudinary: {PublicId}", publicId);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to delete file: {ErrorMessage}", e.Message);
                throw new AppException(ErrorCode.DELETE_FILE_FAILED);
            }
        }

        private string ExtractPublicId(string fileUrl)
        {
            try
            {
                // Lấy public_id từ URL (bỏ phần domain và version)
                var uri = new Uri(fileUrl);
                var path = uri.AbsolutePath;

                // Tìm vị trí của "/upload/"
                var uploadIndex = path.IndexOf("/upload/");
                if (uploadIndex == -1)
                {
                    throw new ArgumentException("Invalid Cloudinary URL format");
                }

                // Lấy phần sau "/upload/"
                var afterUpload = path.Substring(uploadIndex + "/upload/".Length);
                var parts = afterUpload.Split('/');

                // Bỏ qua version (v1234567890) nếu có
                var publicIdParts = new List<string>();
                for (int i = 1; i < parts.Length; i++) // Bỏ qua phần đầu (thường là version)
                {
                    publicIdParts.Add(parts[i]);
                }

                var publicIdWithExtension = string.Join("/", publicIdParts);

                // Bỏ extension (.jpg, .png, etc.)
                var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
                if (lastDotIndex > 0)
                {
                    return publicIdWithExtension.Substring(0, lastDotIndex);
                }

                return publicIdWithExtension;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to extract public_id from URL {Url}: {Error}", fileUrl, e.Message);
                throw new ArgumentException("Invalid Cloudinary URL", e);
            }
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new AppException(ErrorCode.FILE_EMPTY);
            }

            if (file.Length > MAX_FILE_SIZE)
            {
                throw new AppException(ErrorCode.FILE_TOO_LARGE);
            }

            string? contentType = file.ContentType;
            if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("image/"))
            {
                throw new AppException(ErrorCode.INVALID_FILE_TYPE);
            }
        }
    }
}