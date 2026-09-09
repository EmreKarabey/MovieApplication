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
using CoreSecurity.Entities;
using Domain.Entities;
using MediatR;

namespace Application.Features.LikedMovie.Queries.GetMovieLikedCount
{
    public class GetMovieLikedCountQuery : IRequest<int>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }

        public string? CacheKey => $"GetMovieLikedCountQuery Movie_Id:{MovieID}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "LikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetMovieLikedCountHandler : IRequestHandler<GetMovieLikedCountQuery, int>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;

        public GetMovieLikedCountHandler(ILikedMovieRepository likedMovieRepository)
        {
            _likedMovieRepository = likedMovieRepository;
        }

        public async Task<int> Handle(GetMovieLikedCountQuery request, CancellationToken cancellationToken)
        {
            int count = await _likedMovieRepository.MovieLikedCount(predicate: n => n.MovieID == request.MovieID, cancellationToken: cancellationToken);

            return count;
        }
    }
}
