using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Queries.MyFavoriteMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FavoriteMovie.Queries.SearchFavoriteMovie
{
    public class GetSearchFavoriteMovieQuery : IRequest<GetListResponse<GetSearchFavoriteMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public string Search { get; set; }
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string? CacheKey => $"GetSearchFavoriteMovieQuery Search:{Search} UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetSearchFavoriteMovieHandler : IRequestHandler<GetSearchFavoriteMovieQuery, GetListResponse<GetSearchFavoriteMovieDto>>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;

        public GetSearchFavoriteMovieHandler(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<GetListResponse<GetSearchFavoriteMovieDto>> Handle(GetSearchFavoriteMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.FavoriteMovie, bool>> expression = n => n.UserID == request.UserId;

            if (!string.IsNullOrEmpty(request.Search))
                expression = n => n.UserID == request.UserId && n.Movie.Name.Contains(request.Search);

            var entity = await _favoriteMovieRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchFavoriteMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchFavoriteMovieDto getSearchFavoriteMovieDto = new()
                {
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    EntityID = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL
                };

                getListResponse.Items.Add(getSearchFavoriteMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
