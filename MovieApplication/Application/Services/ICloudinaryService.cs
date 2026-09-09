using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface ICloudinaryService
    {
        public Task<string> UploadAsync(IFormFile file);
        public Task DeleteAsync(string publicId);

        public Task<string> UploadImageAsync(IFormFile file);
        public Task DeleteImageAsync(string publicId);
    }
}
