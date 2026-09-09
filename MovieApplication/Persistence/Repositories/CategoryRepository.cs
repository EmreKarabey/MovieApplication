using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class CategoryRepository : EFRepositoryBase<Category, Guid, BaseDBContext>, ICategoryRepository
    {
        public CategoryRepository(BaseDBContext context) : base(context)
        {
        }
    }
}
