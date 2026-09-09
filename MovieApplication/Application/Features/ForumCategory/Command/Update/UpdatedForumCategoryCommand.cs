using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ForumCategory.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.ForumCategory.Command.Update
{
    public class UpdatedForumCategoryCommand : IRequest<UpdatedForumCategoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string? CacheKey => $"UpdatedForumCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "ForumCategories";
    }

    public class UpdatedForumCategoryHandler : IRequestHandler<UpdatedForumCategoryCommand, UpdatedForumCategoryResponse>
    {
        private readonly ForumCategoryBusinessRules _forumCategoryBusinessRules;
        private readonly IForumCategoryRepository _forumCategoryRepository;
        private readonly IMapper _mapper;

        public UpdatedForumCategoryHandler(IForumCategoryRepository forumCategoryRepository, IMapper mapper, ForumCategoryBusinessRules forumCategoryBusinessRules)
        {
            _forumCategoryRepository = forumCategoryRepository;
            _mapper = mapper;
            _forumCategoryBusinessRules = forumCategoryBusinessRules;
        }

        public async Task<UpdatedForumCategoryResponse> Handle(UpdatedForumCategoryCommand request, CancellationToken cancellationToken)
        {
            await _forumCategoryBusinessRules.NoForumCategoryFound(request.Id);

            await _forumCategoryBusinessRules.ForumCategoryNameCannotBeDublicatedWhenInserted(request.Name);

            Domain.Entities.ForumCategory? forumCategory = await _forumCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            forumCategory = _mapper.Map(request, forumCategory);

            var updatedEntity = await _forumCategoryRepository.UpdateAsync(forumCategory);

            var result = _mapper.Map<UpdatedForumCategoryResponse>(updatedEntity);

            return result;
        }
    }
}
