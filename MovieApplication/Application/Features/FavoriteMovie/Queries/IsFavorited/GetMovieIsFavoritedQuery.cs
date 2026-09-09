using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Queries.IsFavorited
{
    public class GetMovieIsFavoritedQuery : IRequest<bool>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"GetMovieIsFavoritedQuery Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMovieIsLikedHandler : IRequestHandler<GetMovieIsFavoritedQuery, bool>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly LoginBusinessRules _loginBusinessRules;

        public GetMovieIsLikedHandler(IFavoriteMovieRepository favoriteMovieRepository, LoginBusinessRules loginBusinessRules)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<bool> Handle(GetMovieIsFavoritedQuery request, CancellationToken cancellationToken)
        {
            await _loginBusinessRules.NoUserFound(request.UserId);
            var result = await _favoriteMovieRepository.AnyAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);
            return result;
        }
    }
}
