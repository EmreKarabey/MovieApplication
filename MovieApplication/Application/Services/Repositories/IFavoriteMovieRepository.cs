using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories
{
    public interface IFavoriteMovieRepository : IAsyncRepository<FavoriteMovie, Guid>
    {
        public Task<int> MovieFavoritedCount(Expression<Func<FavoriteMovie, bool>> predicate, CancellationToken cancellationToken = default);
        public Task<int> MyMovieFavoritedCount(int UserId);
    }
}
