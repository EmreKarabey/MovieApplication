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
    public class CommentRepository : EFRepositoryBase<Comments, Guid, BaseDBContext>, ICommentRepository
    {
        public CommentRepository(BaseDBContext context) : base(context)
        {
        }
    }
}
