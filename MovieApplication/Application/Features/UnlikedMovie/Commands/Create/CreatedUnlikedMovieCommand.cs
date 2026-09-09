using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.UnlikedMovie.Commands.Create
{
    public class CreatedUnlikedMovieCommand : IRequest<CreatedUnlikedMovieResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"CreatedUnlikedMovieCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";
    }

    public class CreatedUnlikedMovieHandler : IRequestHandler<CreatedUnlikedMovieCommand, CreatedUnlikedMovieResponse>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;

        public CreatedUnlikedMovieHandler(IUnlikedMovieRepository unlikedMovieRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedUnlikedMovieResponse> Handle(CreatedUnlikedMovieCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.UnlikedMovie>(request);

            entity.UserID = currentUserId;

            var addlikedMovie = await _unlikedMovieRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedUnlikedMovieResponse>(addlikedMovie);

            return result;
        }
    }
}