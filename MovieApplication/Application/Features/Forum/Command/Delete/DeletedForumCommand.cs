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

namespace Application.Features.Forum.Command.Delete
{
    public class DeletedForumCommand : IRequest<DeletedForumResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"DeletedForumCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";
    }

    public class DeletedForumHandler : IRequestHandler<DeletedForumCommand, DeletedForumResponse>
    {
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;
        private readonly ForumBusinessRules _forumBusinessRules;
        public DeletedForumHandler(IForumRepository forumRepository, IMapper mapper, ForumBusinessRules forumBusinessRules)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
            _forumBusinessRules = forumBusinessRules;
        }

        public async Task<DeletedForumResponse> Handle(DeletedForumCommand request, CancellationToken cancellationToken)
        {
            await _forumBusinessRules.NoForumFound(request.Id);

            Domain.Entities.Forum forum = await _forumRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteForum = await _forumRepository.DeleteAsync(forum);

            var result = _mapper.Map<DeletedForumResponse>(deleteForum);

            return result;
        }
    }
}
