using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.UnlikedMovie.Queries.IsLiked
{
    public class GetMovieIsUnlikedQuery : IRequest<bool>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"GetMovieIsUnlikedQuery Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "UnlikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMovieIsLikedHandler : IRequestHandler<GetMovieIsUnlikedQuery, bool>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly LoginBusinessRules _loginBusinessRules;

        public GetMovieIsLikedHandler(IUnlikedMovieRepository unlikedMovieRepository, LoginBusinessRules loginBusinessRules)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<bool> Handle(GetMovieIsUnlikedQuery request, CancellationToken cancellationToken)
        {
            await _loginBusinessRules.NoUserFound(request.UserId);
            var result = await _unlikedMovieRepository.AnyAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);
            return result;
        }
    }
}
