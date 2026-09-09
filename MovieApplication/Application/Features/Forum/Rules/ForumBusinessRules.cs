using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Forum.Rules
{
    public class ForumBusinessRules : BaseBusinessRules
    {
        private readonly IForumRepository _forumRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForumBusinessRules(IForumRepository forumRepository, IHttpContextAccessor httpContextAccessor)
        {
            _forumRepository = forumRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoForumFound(Guid Id)
        {
            Domain.Entities.Forum forum = await _forumRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (forum == null) throw new BusinessException(ForumMessages.NoForumFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);

            Domain.Entities.Forum forum = await _forumRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (forum.UserId != currentUserId) throw new BusinessException(ForumMessages.NotBeVerified);
        }
    }
}
