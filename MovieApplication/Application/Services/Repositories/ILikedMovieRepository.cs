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
    public interface ILikedMovieRepository : IAsyncRepository<LikedMovie, Guid>
    {
        public Task<int> MovieLikedCount(Expression<Func<LikedMovie, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
