using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Constants;
using Application.Features.LikedMovie.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.FavoriteMovie.Rules
{
    public class FavoriteMovieBusinessRules
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteMovieBusinessRules(IFavoriteMovieRepository favoriteMovieRepository, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoFavoriteMovieFound(Guid Id)
        {
            var favoriteMovie = await _favoriteMovieRepository.GetAsync(predicate: n => n.MovieID == Id);

            if (favoriteMovie == null) throw new BusinessException(FavoriteMovieMessages.NoFavoriteMovieFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Domain.Entities.FavoriteMovie favoriteMovie = await _favoriteMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (favoriteMovie.UserID != currentUserId) throw new BusinessException(FavoriteMovieMessages.NotBeVerified);
        }
    }
}
