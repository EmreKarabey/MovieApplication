using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Queries.MyFavoriteMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.History.Queries.MyHistory
{
    public class GetMyHistoryQuery : IRequest<GetListResponse<GetMyHistoryDto>>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string CacheKey => $"MyHistoryQuery{UserId}_{pageRequest.PageIndex}";

        public string? CacheGroupKey => "Histories";

        public bool ByPassCache => true;

        public TimeSpan? SlidingExpiration { get; }
    }

    public class MySavedMovieHandler : IRequestHandler<GetMyHistoryQuery, GetListResponse<GetMyHistoryDto>>
    {
        private readonly IHistoryRepository _historyRepository;

        public MySavedMovieHandler(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<GetListResponse<GetMyHistoryDto>> Handle(GetMyHistoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _historyRepository.GetListAsync(predicate: n => n.UserID == request.UserId
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie).ThenInclude(n => n.MoviesCategories).ThenInclude(n => n.Category),
            orderBy: n => n.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);

            GetListResponse<GetMyHistoryDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetMyHistoryDto getMyHistoryDto = new()
                {
                    EntityID = item.EntityID,
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    MovieId = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL,
                    CreatedAt = item.CreatedAt,
                    CategoryName = item.Movie.MoviesCategories.Select(n => n.Category.CategoryName),
                    TotalSeconds = item.TotalSeconds,
                    WatchedSeconds = item.WatchedSeconds
                };

                getListResponse.Items.Add(getMyHistoryDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
