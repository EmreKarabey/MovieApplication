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

namespace Application.Features.ForumCategory.Queries.GetById
{
    public class GetByIdForumCategoryQuery : IRequest<GetByIdForumCategoryDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"GetByIdForumCategoryQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "ForumCategories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdForumCategoryHandler : IRequestHandler<GetByIdForumCategoryQuery, GetByIdForumCategoryDto>
    {
        private readonly IForumCategoryRepository _forumCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ForumCategoryBusinessRules _forumCategoryBusinessRules;

        public GetByIdForumCategoryHandler(IForumCategoryRepository forumCategoryRepository, IMapper mapper, ForumCategoryBusinessRules forumCategoryBusinessRules)
        {
            _forumCategoryRepository = forumCategoryRepository;
            _mapper = mapper;
            _forumCategoryBusinessRules = forumCategoryBusinessRules;
        }

        public async Task<GetByIdForumCategoryDto> Handle(GetByIdForumCategoryQuery request, CancellationToken cancellationToken)
        {
            await _forumCategoryBusinessRules.NoForumCategoryFound(request.Id);

            Domain.Entities.ForumCategory forumCategory = await _forumCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdForumCategoryDto>(forumCategory);

            return result;
        }
    }
}
