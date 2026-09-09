using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Queries.MyFavoriteMovie;
using Application.Features.FavoriteMovie.Queries.SearchFavoriteMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SavedMovie.Queries.SearchSaveMovie
{
    public class GetSearchSaveMovieQuery : IRequest<GetListResponse<GetSearchSaveMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public string Search { get; set; }

        public PageRequest pageRequest { get; set; }

        public int UserId { get; set; }
        public string? CacheKey => $"GetSearchSaveMovieQuery Search:{Search} UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetSearchSaveMovieHandler : IRequestHandler<GetSearchSaveMovieQuery, GetListResponse<GetSearchSaveMovieDto>>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;

        public GetSearchSaveMovieHandler(ISavedMovieRepository savedMovieRepository)
        {
            _savedMovieRepository = savedMovieRepository;
        }

        public async Task<GetListResponse<GetSearchSaveMovieDto>> Handle(GetSearchSaveMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.SavedMovie, bool>> expression = n => n.UserID == request.UserId;

            if (!string.IsNullOrEmpty(request.Search)) expression = n => n.Movie.Name.Contains(request.Search) && n.UserID == request.UserId;

            var entity = await _savedMovieRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchSaveMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchSaveMovieDto getSearchSaveMovieDto = new()
                {
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    EntityID = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL
                };

                getListResponse.Items.Add(getSearchSaveMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
