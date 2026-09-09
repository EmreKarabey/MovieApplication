using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSecurity.Entities;

namespace Application.Features.Register
{
    public class RegisterResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
