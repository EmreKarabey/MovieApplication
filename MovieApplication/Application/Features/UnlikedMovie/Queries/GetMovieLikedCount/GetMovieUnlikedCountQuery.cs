using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using MediatR;

namespace Application.Features.UnlikedMovie.Queries.GetMovieLikedCount
{
    public class GetMovieUnlikedCountQuery : IRequest<int>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"GetMovieUnlikedCountQuery(Id={MovieID})";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "UnlikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetMovieLikedCountHandler : IRequestHandler<GetMovieUnlikedCountQuery, int>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;

        public GetMovieLikedCountHandler(IUnlikedMovieRepository unlikedMovieRepository)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
        }

        public async Task<int> Handle(GetMovieUnlikedCountQuery request, CancellationToken cancellationToken)
        {
            int count = await _unlikedMovieRepository.MovieUnlikedCount(predicate: n => n.MovieID == request.MovieID, cancellationToken: cancellationToken);

            return count;
        }
    }
}
