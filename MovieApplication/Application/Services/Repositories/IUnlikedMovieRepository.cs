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
    public interface IUnlikedMovieRepository : IAsyncRepository<UnlikedMovie, Guid>
    {
        public Task<int> MovieUnlikedCount(Expression<Func<UnlikedMovie, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
