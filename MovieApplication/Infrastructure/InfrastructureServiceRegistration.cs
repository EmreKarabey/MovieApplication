using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.Services.GoogleLoginService;
using Application.Services.HelsinkiService;
using Application.Services.ToxicBertService;
using Infrastructure.Services.Cloudinary;
using Infrastructure.Services.CloudinaryService;
using Infrastructure.Services.Email;
using Infrastructure.Services.GoogleLogin;
using Infrastructure.Services.Helsinki;
using Infrastructure.Services.Hmac;
using Infrastructure.Services.ToxicBert;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddSingleton<HmacService>();
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            services.AddHttpClient();
            services.AddScoped<IToxicBertService, ToxicBertService>();
            services.AddScoped<IHelsinkiService, HelsinkiService>();
            services.AddScoped<IGoogleLoginService, GoogleLoginService>();

            return services;
        }
    }
}
