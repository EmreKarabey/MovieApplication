using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Forum.Command.Update
{
    public class UpdateForumCommand : IRequest<UpdateForumResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool Status { get; set; }
        public Guid ForumCategoryId { get; set; }

        public string? CacheKey => $"UpdatedForumCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";
    }
    public class UpdateForumHandler : IRequestHandler<UpdateForumCommand, UpdateForumResponse>
    {
        private readonly ForumBusinessRules _forumBusinessRules;
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;

        public UpdateForumHandler(IForumRepository forumRepository, IMapper mapper, ForumBusinessRules forumBusinessRules)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
            _forumBusinessRules = forumBusinessRules;
        }

        public async Task<UpdateForumResponse> Handle(UpdateForumCommand request, CancellationToken cancellationToken)
        {

            await _forumBusinessRules.NoForumFound(request.Id);

            await _forumBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.Forum forum = await _forumRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            forum = _mapper.Map(request, forum);

            var updateForum = await _forumRepository.UpdateAsync(forum);

            var result = _mapper.Map<UpdateForumResponse>(updateForum);

            return result;
        }
    }
}
