using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SavedMovie.Queries.MySavedMovie
{
    public class GetMySavedMovieQuery : IRequest<GetListResponse<GetMySavedMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string CacheKey => $"MySavedMovieQuery_{UserId}_{pageRequest.PageIndex}";

        public string? CacheGroupKey => "SavedMovies";

        public bool ByPassCache => true;

        public TimeSpan? SlidingExpiration { get; }
    }

    public class MySavedMovieHandler : IRequestHandler<GetMySavedMovieQuery, GetListResponse<GetMySavedMovieDto>>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;

        public MySavedMovieHandler(ISavedMovieRepository savedMovieRepository)
        {
            _savedMovieRepository = savedMovieRepository;
        }

        public async Task<GetListResponse<GetMySavedMovieDto>> Handle(GetMySavedMovieQuery request, CancellationToken cancellationToken)
        {
            var entity = await _savedMovieRepository.GetListAsync(predicate: n => n.UserID == request.UserId
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetMySavedMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetMySavedMovieDto getMySavedMovieDto = new()
                {
                    ImageURL = item.Movie.ImageURL,
                    Description = item.Movie.Description,
                    EntityID = item.MovieID,
                    Name = item.Movie.Name,
                    ProducerName = item.Movie.ProducerName,
                    ReleaseDate = item.Movie.ReleaseDate,
                    VideoURL = item.Movie.VideoURL
                };

                getListResponse.Items.Add(getMySavedMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
