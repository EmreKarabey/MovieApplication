using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.JWT;
using CoreSecurity.OTPAuthenticator;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSecurity
{
    public static class CoreSecurityServiceRegistration
    {
        public static IServiceCollection AddCoreSecurityService(this IServiceCollection services)
        {
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IOTPAuthenticator, OTPNetAuthenticatorHelper>();

            return services;
        }
    }
}
