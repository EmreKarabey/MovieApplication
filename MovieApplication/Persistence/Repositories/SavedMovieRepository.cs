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
    public class SavedMovieRepository : EFRepositoryBase<SavedMovie, Guid, BaseDBContext>, ISavedMovieRepository
    {
        public SavedMovieRepository(BaseDBContext context) : base(context)
        {
        }

        public async Task<int> MyMovieSavedCount(int UserId)
        {
            IQueryable<SavedMovie> list = Query();

            var result = await list.Where(n => n.UserID == UserId).CountAsync();

            return result;
        }
    }
}
