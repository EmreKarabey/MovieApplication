using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ForumCategory.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;

namespace Application.Features.ForumCategory.Rules
{
    public class ForumCategoryBusinessRules : BaseBusinessRules
    {
        private readonly IForumCategoryRepository _forumCategoryRepository;

        public ForumCategoryBusinessRules(IForumCategoryRepository forumCategoryRepository)
        {
            _forumCategoryRepository = forumCategoryRepository;
        }

        public async Task ForumCategoryNameCannotBeDublicatedWhenInserted(string name)
        {
            Domain.Entities.ForumCategory forumCategory = await _forumCategoryRepository.GetAsync(predicate: n => n.Name == name);

            if (forumCategory != null) throw new BusinessException(ForumCategoryMessages.ForumCategoryNameExists);
        }

        public async Task NoForumCategoryFound(Guid Id)
        {
            Domain.Entities.ForumCategory forumCategory = await _forumCategoryRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (forumCategory == null) throw new BusinessException(ForumCategoryMessages.NoForumCategoryFound);
        }
    }
}
