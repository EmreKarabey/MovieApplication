using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Queries.SearchLikedMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UnlikedMovie.Queries.SearchUnlikedMovie
{
    public class GetSearchUnlikedMovieQuery : IRequest<GetListResponse<GetSearchUnlikedMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public string Search { get; set; }
        public int UserId { get; set; }
        public PageRequest pageRequest { get; set; }
        public string? CacheKey => $"GetSearchUnlikedMovieQuery Search:{Search} UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }
    public class GetSearchUnlikedMovieHandler : IRequestHandler<GetSearchUnlikedMovieQuery, GetListResponse<GetSearchUnlikedMovieDto>>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;

        public GetSearchUnlikedMovieHandler(IUnlikedMovieRepository unlikedMovieRepository)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
        }

        public async Task<GetListResponse<GetSearchUnlikedMovieDto>> Handle(GetSearchUnlikedMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.UnlikedMovie, bool>> expression = n => n.UserID == request.UserId;

            if (!string.IsNullOrEmpty(request.Search))
                expression = n => n.UserID == request.UserId && n.Movie.Name.Contains(request.Search);

            var entity = await _unlikedMovieRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            include: n => n.Include(p => p.Movie),
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchUnlikedMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchUnlikedMovieDto getSearchUnlikedMovieDto = new()
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

                getListResponse.Items.Add(getSearchUnlikedMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
