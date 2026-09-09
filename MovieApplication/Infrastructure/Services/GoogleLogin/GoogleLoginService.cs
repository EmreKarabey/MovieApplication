using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.GoogleLoginService;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.GoogleLogin
{
    public class GoogleLoginService : IGoogleLoginService
    {
        private readonly IConfiguration _config;

        public GoogleLoginService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<GoogleJsonWebSignature.Payload> GoogleLoginAsync(string Token)
        {
            GoogleJsonWebSignature.Payload payload;
            var ClientID = _config["Google:ClientID"] ?? throw new InvalidOperationException("Google:ClientID ayarı `appsettings.json` dosyasında bulunamadı.");
            var ClientSecret = _config["Google:ClientSecret"] ?? throw new InvalidOperationException("Google:ClientSecret ayarı `appsettings.json` dosyasında bulunamadı.");
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { "1470086286-ofqupc88osouibg41c66kg5t7j4b94n4.apps.googleusercontent.com" }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(Token, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Geçersiz Google Token!");
            }
        }
    }
}
