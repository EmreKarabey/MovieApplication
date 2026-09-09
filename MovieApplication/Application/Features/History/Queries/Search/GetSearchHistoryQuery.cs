using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Queries.GetList;
using Application.Features.History.Queries.MyHistory;
using Application.Features.SavedMovie.Queries.SearchSaveMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.History.Queries.Search
{
    public class GetSearchHistoryQuery : IRequest<GetListResponse<GetSearchHistoryDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;

        public string Search { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"GetSearchSaveMovieQuery Search:{Search} UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetSearchHistoryHandler : IRequestHandler<GetSearchHistoryQuery, GetListResponse<GetSearchHistoryDto>>
    {
        private readonly IHistoryRepository _historyRepository;

        public GetSearchHistoryHandler(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<GetListResponse<GetSearchHistoryDto>> Handle(GetSearchHistoryQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.History, bool>> expression = n => n.UserID == request.UserId;

            if (!string.IsNullOrEmpty(request.Search)) expression = n => n.Movie.Name.Contains(request.Search) && n.UserID == request.UserId;

            var entity = await _historyRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie).ThenInclude(n => n.MoviesCategories).ThenInclude(n => n.Category),
            orderBy: n => n.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchHistoryDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchHistoryDto getSearchHistoryDto = new()
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

                getListResponse.Items.Add(getSearchHistoryDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
