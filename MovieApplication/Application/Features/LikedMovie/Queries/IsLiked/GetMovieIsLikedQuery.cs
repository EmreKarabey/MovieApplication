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

namespace Application.Features.LikedMovie.Queries.IsLiked
{
    public class GetMovieIsLikedQuery : IRequest<bool>, ICachableRequest, ILoggableRequest
    {
        public Guid MovieId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"GetMovieIsLikedQuery Movie_Id:{MovieId}, User_Id:{UserId}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "LikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMovieIsLikedHandler : IRequestHandler<GetMovieIsLikedQuery, bool>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly LoginBusinessRules _loginBusinessRules;

        public GetMovieIsLikedHandler(ILikedMovieRepository likedMovieRepository, LoginBusinessRules loginBusinessRules)
        {
            _likedMovieRepository = likedMovieRepository;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<bool> Handle(GetMovieIsLikedQuery request, CancellationToken cancellationToken)
        {
            await _loginBusinessRules.NoUserFound(request.UserId);
            var result = await _likedMovieRepository.AnyAsync(predicate: n => n.MovieID == request.MovieId && n.UserID == request.UserId, cancellationToken: cancellationToken);
            return result;
        }
    }
}
