using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Queries.MyLikedMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UnlikedMovie.Queries.MyUnlikedMovies
{
    public class GetMyUnlikedMovieQuery : IRequest<GetListResponse<GetMyUnlikedMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string CacheKey => $"GetMyUnlikedMovieQuery{UserId}_{pageRequest.PageIndex}";

        public string? CacheGroupKey => "UnlikedMovies";

        public bool ByPassCache { get; }

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetMyLikedMovieHandler : IRequestHandler<GetMyUnlikedMovieQuery, GetListResponse<GetMyUnlikedMovieDto>>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;

        public GetMyLikedMovieHandler(IUnlikedMovieRepository unlikedMovieRepository)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
        }

        public async Task<GetListResponse<GetMyUnlikedMovieDto>> Handle(GetMyUnlikedMovieQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unlikedMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie).ThenInclude(n => n.MoviesCategories).ThenInclude(n => n.Category),
            cancellationToken: cancellationToken);

            GetListResponse<GetMyUnlikedMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetMyUnlikedMovieDto getMyUnlikedMovieDto = new()
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

                getListResponse.Items.Add(getMyUnlikedMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
