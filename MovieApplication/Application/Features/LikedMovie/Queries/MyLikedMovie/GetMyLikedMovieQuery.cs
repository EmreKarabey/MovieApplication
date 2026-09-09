using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Queries.MyHistory;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LikedMovie.Queries.MyLikedMovie
{
    public class GetMyLikedMovieQuery : IRequest<GetListResponse<GetMyLikedMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string CacheKey => $"GetMyLikedMovieQuery{UserId}_{pageRequest.PageIndex}";

        public string? CacheGroupKey => "LikedMovies";

        public bool ByPassCache { get; }

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMyLikedMovieHandler : IRequestHandler<GetMyLikedMovieQuery, GetListResponse<GetMyLikedMovieDto>>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;

        public GetMyLikedMovieHandler(ILikedMovieRepository likedMovieRepository)
        {
            _likedMovieRepository = likedMovieRepository;
        }

        public async Task<GetListResponse<GetMyLikedMovieDto>> Handle(GetMyLikedMovieQuery request, CancellationToken cancellationToken)
        {
            var entity = await _likedMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie).ThenInclude(n => n.MoviesCategories).ThenInclude(n => n.Category),
            cancellationToken: cancellationToken);

            GetListResponse<GetMyLikedMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetMyLikedMovieDto getMyLikedMovieDto = new()
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
                    CategoryName = item.Movie.MoviesCategories.Select(n => n.Category.CategoryName)
                };

                getListResponse.Items.Add(getMyLikedMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
