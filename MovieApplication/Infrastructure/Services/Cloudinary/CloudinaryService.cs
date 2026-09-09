using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Infrastructure.Services.Cloudinary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();

            var account = new Account
            {
                ApiKey = settings.ApiKey,
                ApiSecret = settings.ApiSecret,
                Cloud = settings.CloudName
            };

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task DeleteAsync(string publicId)
        {
            var deletedParam = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video
            };

            await _cloudinary.DestroyAsync(deletedParam);
        }

        public async Task DeleteImageAsync(string publicId)
        {
            var deletedParam = new DeletionParams(publicId);

            await _cloudinary.DestroyAsync(deletedParam);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var uploadParam = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "movies"
            };

            var result = await _cloudinary.UploadAsync(uploadParam);

            if (result.Error != null)
                throw new BusinessException($"Cloudinary upload failed: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "movies-images"
            };

            var result = await _cloudinary.UploadAsync(uploadParam);

            if (result.Error != null)
                throw new BusinessException($"Cloudinary upload failed: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }
    }
}
