using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace Application.Services.GoogleLoginService
{
    public interface IGoogleLoginService
    {
        public Task<GoogleJsonWebSignature.Payload> GoogleLoginAsync(string Token);
    }
}
