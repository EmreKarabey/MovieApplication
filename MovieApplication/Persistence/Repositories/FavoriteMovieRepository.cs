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
    public class FavoriteMovieRepository : EFRepositoryBase<FavoriteMovie, Guid, BaseDBContext>, IFavoriteMovieRepository
    {
        public FavoriteMovieRepository(BaseDBContext context) : base(context)
        {
        }

        public async Task<int> MovieFavoritedCount(Expression<Func<FavoriteMovie, bool>> predicate, CancellationToken cancellationToken = default)
        {
            IQueryable<FavoriteMovie> list = Query();

            if (predicate != null) list = list.Where(predicate);

            return await list.CountAsync(cancellationToken);
        }

        public async Task<int> MyMovieFavoritedCount(int UserId)
        {
            IQueryable<FavoriteMovie> list = Query();

            var result = await list.Where(n => n.UserID == UserId).CountAsync();

            return result;
        }
    }
}
