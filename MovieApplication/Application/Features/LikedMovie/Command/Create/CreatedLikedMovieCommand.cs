using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Create;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.LikedMovie.Command.Create
{
    public class CreatedLikedMovieCommand : IRequest<CreatedLikedMovieResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"CreatedLikedMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";
    }

    public class CreatedLikedMovieHandler : IRequestHandler<CreatedLikedMovieCommand, CreatedLikedMovieResponse>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;

        public CreatedLikedMovieHandler(ILikedMovieRepository likedMovieRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _likedMovieRepository = likedMovieRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedLikedMovieResponse> Handle(CreatedLikedMovieCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.LikedMovie>(request);

            entity.UserID = currentUserId;

            var addlikedMovie = await _likedMovieRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedLikedMovieResponse>(addlikedMovie);

            return result;
        }
    }
}
