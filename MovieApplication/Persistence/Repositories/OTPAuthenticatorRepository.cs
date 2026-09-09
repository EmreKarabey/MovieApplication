using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Repositories;
using CoreSecurity.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class OTPAuthenticatorRepository : EFRepositoryBase<OTPAuthenticator, int, BaseDBContext>, IOTPAuthenticatorRepository
    {
        public OTPAuthenticatorRepository(BaseDBContext context) : base(context)
        {
        }
    }
}
