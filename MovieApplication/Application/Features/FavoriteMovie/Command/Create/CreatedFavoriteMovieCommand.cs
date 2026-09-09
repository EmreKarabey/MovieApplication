using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.FavoriteMovie.Command.Create
{
    public class CreatedFavoriteMovieCommand : IRequest<CreatedFavoriteMovieResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest
    {
        public Guid MovieID { get; set; }


        public string? CacheKey => "CreateFavoriteMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";
    }

    public class CreateFavoriteMovieHandler : IRequestHandler<CreatedFavoriteMovieCommand, CreatedFavoriteMovieResponse>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IMapper _mapper;

        public CreateFavoriteMovieHandler(IFavoriteMovieRepository favoriteMovieRepository, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules, IMapper mapper)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
            _mapper = mapper;
        }

        public async Task<CreatedFavoriteMovieResponse> Handle(CreatedFavoriteMovieCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
      .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.FavoriteMovie>(request);

            entity.UserID = currentUserId;

            var result = await _favoriteMovieRepository.AddAsync(entity);

            return _mapper.Map<CreatedFavoriteMovieResponse>(result);
        }
    }
}
