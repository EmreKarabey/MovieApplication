using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Create;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.SavedMovie.Command.Create
{
    public class CreatedSavedMovieCommand : IRequest<CreatedSavedMovieResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"CreatedSavedMovieCommand Movie_Id:{MovieID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";
    }

    public class CreatedSavedMovieHandler : IRequestHandler<CreatedSavedMovieCommand, CreatedSavedMovieResponse>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;

        public CreatedSavedMovieHandler(ISavedMovieRepository savedMovieRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _savedMovieRepository = savedMovieRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedSavedMovieResponse> Handle(CreatedSavedMovieCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.SavedMovie>(request);

            entity.UserID = currentUserId;

            var addSavedMovie = await _savedMovieRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedSavedMovieResponse>(addSavedMovie);

            return result;
        }
    }
}
