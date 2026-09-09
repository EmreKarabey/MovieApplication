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
    public interface ISavedMovieRepository : IAsyncRepository<SavedMovie, Guid>
    {
        public Task<int> MyMovieSavedCount(int UserId);
    }
}
