using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Constants;
using Application.Features.LikedMovie.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.LikedMovie.Rules
{
    public class LikedMovieBusinessRules
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LikedMovieBusinessRules(ILikedMovieRepository likedMovieRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likedMovieRepository = likedMovieRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoLikedMovieFound(Guid Id)
        {
            Domain.Entities.LikedMovie likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (likedMovie == null) throw new BusinessException(LikedMovieMessages.NoLikedMovieFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Domain.Entities.LikedMovie likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (likedMovie.UserID != currentUserId) throw new BusinessException(LikedMovieMessages.NotBeVerified);
        }

        public async Task NoMovieFound(Guid Id)
        {
            Domain.Entities.LikedMovie likedMovie = await _likedMovieRepository.GetAsync(predicate: n => n.MovieID == Id);

            if (likedMovie == null) throw new BusinessException(LikedMovieMessages.NoMovieFound);
        }
    }
}
