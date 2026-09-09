using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.FavoriteMovie.Queries.GetMovieFavoritedCount
{
    public class GetMovieFavoritedCountQuery : IRequest<int>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"GetMovieFavoritedCountQuery Movie_Id:{MovieID}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetMovieFavoritedCountHandler : IRequestHandler<GetMovieFavoritedCountQuery, int>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;

        public GetMovieFavoritedCountHandler(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<int> Handle(GetMovieFavoritedCountQuery request, CancellationToken cancellationToken)
        {
            int count = await _favoriteMovieRepository.MovieFavoritedCount(predicate: n => n.MovieID == request.MovieID, cancellationToken: cancellationToken);

            return count;
        }
    }
}
