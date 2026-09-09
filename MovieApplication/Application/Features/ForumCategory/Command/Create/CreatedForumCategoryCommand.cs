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
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.ForumCategory.Command.Create
{
    public class CreatedForumCategoryCommand : IRequest<CreatedForumCategoryResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest
    {
        public string Name { get; set; }

        public string? CacheKey => $"CreatedForumCategoryCommand Name:{Name}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "ForumCategories";
    }

    public class CreateForumCategoryHandler : IRequestHandler<CreatedForumCategoryCommand, CreatedForumCategoryResponse>
    {
        private readonly ForumCategoryBusinessRules _forumCategoryBusinessRules;
        private readonly IForumCategoryRepository _forumCategoryRepository;
        private readonly IMapper _mapper;

        public CreateForumCategoryHandler(IForumCategoryRepository forumCategoryRepository, IMapper mapper, ForumCategoryBusinessRules forumCategoryBusinessRules)
        {
            _forumCategoryRepository = forumCategoryRepository;
            _mapper = mapper;
            _forumCategoryBusinessRules = forumCategoryBusinessRules;
        }

        public async Task<CreatedForumCategoryResponse> Handle(CreatedForumCategoryCommand request, CancellationToken cancellationToken)
        {
            await _forumCategoryBusinessRules.ForumCategoryNameCannotBeDublicatedWhenInserted(request.Name);

            var entity = _mapper.Map<Domain.Entities.ForumCategory>(request);

            var addForumCategory = await _forumCategoryRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedForumCategoryResponse>(addForumCategory);

            return result;
        }
    }
}
