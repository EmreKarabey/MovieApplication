using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Comment.Rules
{
    public class CommentBusinessRules : BaseBusinessRules
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentBusinessRules(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoCommentFound(Guid Id)
        {
            Comments comments = await _commentRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (comments == null) throw new BusinessException(CommentMessages.NoCommentFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Comments comments = await _commentRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (comments.UserID != currentUserId) throw new BusinessException(CommentMessages.NotBeVerified);
        }

        public async Task NoMovieFound(Guid Id)
        {
            Comments comments = await _commentRepository.GetAsync(predicate: n => n.MovieID == Id);

            if (comments == null) throw new BusinessException(CommentMessages.NoMovieFound);
        }
    }
}
