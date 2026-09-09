using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Queries.SearchFavoriteMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LikedMovie.Queries.SearchLikedMovie
{
    public class GetSearchLikedMovieQuery : IRequest<GetListResponse<GetSearchLikedMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public string Search { get; set; }
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string? CacheKey => $"GetSearchLikedMovieQuery Search:{Search} UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetSearchLikedMovieHandler : IRequestHandler<GetSearchLikedMovieQuery, GetListResponse<GetSearchLikedMovieDto>>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;

        public GetSearchLikedMovieHandler(ILikedMovieRepository likedMovieRepository)
        {
            _likedMovieRepository = likedMovieRepository;
        }

        public async Task<GetListResponse<GetSearchLikedMovieDto>> Handle(GetSearchLikedMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.LikedMovie, bool>> expression = n => n.UserID == request.UserId;

            if (!string.IsNullOrEmpty(request.Search))
                expression = n => n.UserID == request.UserId && n.Movie.Name.Contains(request.Search);

            var entity = await _likedMovieRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchLikedMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchLikedMovieDto getSearchLikedMovieDto = new()
                {
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    EntityID = item.EntityID,
                    MovieId = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL,
                    CategoryName = Enumerable.Empty<string>()
                };

                getListResponse.Items.Add(getSearchLikedMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
