using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SubComments.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SubComments.Rules
{
    public class SubCommentBusinessRules : BaseBusinessRules
    {
        private readonly ISubCommentRepository _subCommentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubCommentBusinessRules(ISubCommentRepository subCommentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _subCommentRepository = subCommentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoSubCommentFound(Guid Id)
        {
            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (comments == null) throw new BusinessException(SubCommentMessages.NoSubCommentFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (comments.UserID != currentUserId) throw new BusinessException(SubCommentMessages.NotBeVerified);
        }

        public async Task NoCommentFound(Guid Id)
        {
            SubComment comments = await _subCommentRepository.GetAsync(predicate: n => n.CommentID == Id);

            if (comments == null) throw new BusinessException(SubCommentMessages.NoCommentFound);
        }
    }
}




