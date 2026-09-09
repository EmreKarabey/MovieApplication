using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Application.Services.Repositories
{
    public interface IUserRepository : IAsyncRepository<User, int>
    {
    }
}
