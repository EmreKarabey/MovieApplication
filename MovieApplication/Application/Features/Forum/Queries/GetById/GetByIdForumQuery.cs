using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Forum.Queries.GetById
{
    public class GetByIdForumQuery : IRequest<GetByIdForumDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"GetByIdForumQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdForumHandler : IRequestHandler<GetByIdForumQuery, GetByIdForumDto>
    {
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;
        private readonly ForumBusinessRules _forumBusinessRules;
        public GetByIdForumHandler(IForumRepository forumRepository, IMapper mapper, ForumBusinessRules forumBusinessRules)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
            _forumBusinessRules = forumBusinessRules;
        }

        public async Task<GetByIdForumDto> Handle(GetByIdForumQuery request, CancellationToken cancellationToken)
        {
            await _forumBusinessRules.NoForumFound(request.Id);

            Domain.Entities.Forum forum = await _forumRepository.GetAsync(predicate: n => n.EntityID == request.Id, include: n => n.Include(p => p.User).Include(n => n.ForumCategory));

            var result = new GetByIdForumDto
            {
                CreatedAt = forum.CreatedAt,
                Details = forum.Details,
                EntityID = forum.EntityID,
                ForumCategoryId = forum.ForumCategoryId,
                UserName = forum.User.FirstName + " " + forum.User.LastName,
                Title = forum.Title,
                UserId = forum.UserId,
                ForumCategorName = forum.ForumCategory.Name
            };

            return result;
        }
    }
}
