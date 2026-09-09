using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Features.Movies.Queries.GetById;
using Application.Features.Movies.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.History.Queries.MyStatics
{
    public class GetMyStaticsMovieQuery : IRequest<GetMyStaticsMovieDto>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }

        public string CacheKey => $"GetMyStaticsMovieQuery UserId:{UserId}";

        public string? CacheGroupKey => "Movies";

        public bool ByPassCache => true;
        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMyStaticsMovieHandler : IRequestHandler<GetMyStaticsMovieQuery, GetMyStaticsMovieDto>
    {
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IMovieRepository _movieRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;

        public GetMyStaticsMovieHandler(IMovieRepository movieRepository, IMapper mapper, LoginBusinessRules loginBusinessRules, IHistoryRepository historyRepository, IFavoriteMovieRepository favoriteMovieRepository, ISavedMovieRepository savedMovieRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _loginBusinessRules = loginBusinessRules;
            _historyRepository = historyRepository;
            _favoriteMovieRepository = favoriteMovieRepository;
            _savedMovieRepository = savedMovieRepository;
        }

        public async Task<GetMyStaticsMovieDto> Handle(GetMyStaticsMovieQuery request, CancellationToken cancellationToken)
        {
            await _loginBusinessRules.NoUserFound(request.UserId);

            int watchHoursMovieCount = await _historyRepository.WatchedHoursMovie(request.UserId);

            int favoritedMoiveCount = await _favoriteMovieRepository.MyMovieFavoritedCount(request.UserId);

            int savedMovieCount = await _savedMovieRepository.MyMovieSavedCount(request.UserId);

            int watchedMovieCount = await _historyRepository.WatchedMovieCount(request.UserId);


            var result = new GetMyStaticsMovieDto
            {
                SaveMovieCount = savedMovieCount,
                FavoriteMovieCount = favoritedMoiveCount,
                MovieHoursCount = watchHoursMovieCount,
                WatchMovieCount = watchedMovieCount,
            };

            return result;
        }
    }
}
