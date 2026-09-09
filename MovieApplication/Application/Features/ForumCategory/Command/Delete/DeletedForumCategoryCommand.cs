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

namespace Application.Features.ForumCategory.Command.Delete
{
    public class DeletedForumCategoryCommand : IRequest<DeletedForumCategoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedForumCategoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "ForumCategories";
    }

    public class DeletedForumCategoryHandler : IRequestHandler<DeletedForumCategoryCommand, DeletedForumCategoryResponse>
    {
        private readonly ForumCategoryBusinessRules _forumCategoryBusinessRules;
        private readonly IForumCategoryRepository _forumCategoryRepository;
        private readonly IMapper _mapper;

        public DeletedForumCategoryHandler(IForumCategoryRepository forumCategoryRepository, IMapper mapper, ForumCategoryBusinessRules forumCategoryBusinessRules)
        {
            _forumCategoryRepository = forumCategoryRepository;
            _mapper = mapper;
            _forumCategoryBusinessRules = forumCategoryBusinessRules;
        }

        public async Task<DeletedForumCategoryResponse> Handle(DeletedForumCategoryCommand request, CancellationToken cancellationToken)
        {
            await _forumCategoryBusinessRules.NoForumCategoryFound(request.Id);

            Domain.Entities.ForumCategory forumCategory = await _forumCategoryRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteEntity = await _forumCategoryRepository.DeleteAsync(forumCategory);

            var result = _mapper.Map<DeletedForumCategoryResponse>(deleteEntity);

            return result;
        }
    }
}
