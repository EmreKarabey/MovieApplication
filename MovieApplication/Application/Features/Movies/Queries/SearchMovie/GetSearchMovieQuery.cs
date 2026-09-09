using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Queries.GetList;
using Application.Features.UnlikedMovie.Queries.SearchUnlikedMovie;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.Movies.Queries.SearchMovie
{
    public class GetSearchMovieQuery : IRequest<GetListResponse<GetSearchMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public string Search { get; set; }
        public PageRequest pageRequest { get; set; }
        public string? CacheKey => $"GetSearchMovieQuery Search:{Search}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Movies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetSearchMovieHandler : IRequestHandler<GetSearchMovieQuery, GetListResponse<GetSearchMovieDto>>
    {
        private readonly IMovieRepository _movieRepository;

        public GetSearchMovieHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<GetListResponse<GetSearchMovieDto>> Handle(GetSearchMovieQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.Movie, bool>> expression = null;

            if (!string.IsNullOrEmpty(request.Search))
                expression = n => n.Name.Contains(request.Search) || n.ProducerName.Contains(request.Search);

            var entity = await _movieRepository.GetListAsync(predicate: expression
            , withDeleted: false, index: request.pageRequest.PageIndex,
            size: request.pageRequest.PageSize, enableTracking: false,
            cancellationToken: cancellationToken);

            GetListResponse<GetSearchMovieDto> getListResponse = new();

            foreach (var item in entity.Items)
            {
                GetSearchMovieDto getSearchMovieDto = new()
                {
                    ImageURL = item.ImageURL,
                    Description = item.Description,
                    EntityID = item.EntityID,
                    Name = item.Name,
                    ProducerName = item.ProducerName,
                    ReleaseDate = item.ReleaseDate,
                    VideoURL = item.VideoURL
                };

                getListResponse.Items.Add(getSearchMovieDto);
            }

            getListResponse.PageIndex = request.pageRequest.PageIndex;
            getListResponse.PageSize = request.pageRequest.PageSize;

            return getListResponse;
        }
    }
}
