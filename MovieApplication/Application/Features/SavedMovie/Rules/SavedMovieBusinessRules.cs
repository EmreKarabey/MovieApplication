using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Constants;
using Application.Features.SavedMovie.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SavedMovie.Rules
{
    public class SavedMovieBusinessRules : BaseBusinessRules
    {
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SavedMovieBusinessRules(ISavedMovieRepository savedMovieRepository, IHttpContextAccessor httpContextAccessor)
        {
            _savedMovieRepository = savedMovieRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoSavedMovieFound(Guid Id)
        {
            Domain.Entities.SavedMovie savedMovie = await _savedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (savedMovie == null) throw new BusinessException(SavedMovieMessages.NoSavedMovieFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Domain.Entities.SavedMovie savedMovie = await _savedMovieRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (savedMovie.UserID != currentUserId) throw new BusinessException(SavedMovieMessages.NotBeVerified);
        }

    }
}
