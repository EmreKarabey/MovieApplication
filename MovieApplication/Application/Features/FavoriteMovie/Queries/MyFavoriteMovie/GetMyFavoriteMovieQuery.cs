using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SavedMovie.Queries.MySavedMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FavoriteMovie.Queries.MyFavoriteMovie
{
    public class GetMyFavoriteMovieQuery : IRequest<GetListResponse<GetMyFavoriteMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string CacheKey => $"GetMyFavoriteMovieQuery{UserId}_{pageRequest.PageIndex}";

        public string? CacheGroupKey => "SavedMovies";

        public bool ByPassCache => true;

        public TimeSpan? SlidingExpiration { get; }
    }

    public class MySavedMovieHandler : IRequestHandler<GetMyFavoriteMovieQuery, GetListResponse<GetMyFavoriteMovieDto>>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;

        public MySavedMovieHandler(IFavoriteMovieRepository favoriteMovieRepository)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
        }

        public async Task<GetListResponse<GetMyFavoriteMovieDto>> Handle(GetMyFavoriteMovieQuery request, CancellationToken cancellationToken)
        {
            var entity = await _favoriteMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetMyFavoriteMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetMyFavoriteMovieDto getMyFavoriteMovieDto = new()
                {
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    EntityID = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL
                };

                getListResponse.Items.Add(getMyFavoriteMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
