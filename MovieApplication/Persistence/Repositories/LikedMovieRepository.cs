using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class LikedMovieRepository : EFRepositoryBase<LikedMovie, Guid, BaseDBContext>, ILikedMovieRepository
    {
        public LikedMovieRepository(BaseDBContext context) : base(context)
        {
        }
        public async Task<int> MovieLikedCount(Expression<Func<LikedMovie, bool>> predicate, CancellationToken cancellationToken = default)
        {
            IQueryable<LikedMovie> list = Query();

            if (predicate != null) list = list.Where(predicate);

            return await list.CountAsync(cancellationToken);
        }
    }
}
