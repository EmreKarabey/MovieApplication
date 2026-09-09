using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Constants;
using Application.Features.UnlikedMovie.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Microsoft.AspNetCore.Http;

namespace Application.Features.UnlikedMovie.Rules
{
    public class UnlikedBusinessRules
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnlikedBusinessRules(IUnlikedMovieRepository unlikedMovieRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoUnlikedMovieFound(Guid Id)
        {
            Domain.Entities.UnlikedMovie unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (unlikedMovie == null) throw new BusinessException(UnlikedMovieMessages.NoUnlikedMovieFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Domain.Entities.UnlikedMovie unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (unlikedMovie.UserID != currentUserId) throw new BusinessException(UnlikedMovieMessages.NotBeVerified);
        }

        public async Task NoMovieFound(Guid Id)
        {
            Domain.Entities.UnlikedMovie unlikedMovie = await _unlikedMovieRepository.GetAsync(predicate: n => n.MovieID == Id);

            if (unlikedMovie == null) throw new BusinessException(UnlikedMovieMessages.NoMovieFound);
        }
    }
}
