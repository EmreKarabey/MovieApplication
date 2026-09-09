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
    public class UserOperationClaimRepository : EFRepositoryBase<UserOperationClaim, int, BaseDBContext>, IUserOperationClaimRepository
    {
        public UserOperationClaimRepository(BaseDBContext context) : base(context)
        {
        }
    }
}
