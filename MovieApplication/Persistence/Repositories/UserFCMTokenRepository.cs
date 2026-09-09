using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class UserFCMTokenRepository : EFRepositoryBase<UserFCMToken, Guid, BaseDBContext>, IUserFCMTokenRepository
    {
        public UserFCMTokenRepository(BaseDBContext context) : base(context)
        {
        }

        public Task<List<string>> Tokens(List<int> UserId)
        {
            var tokens = _context.UserFCMTokens.AsQueryable();

            var result = tokens.Where(n => UserId.Contains(n.UserId)).Select(n => n.Token).ToListAsync();

            return result;
        }
    }
}
