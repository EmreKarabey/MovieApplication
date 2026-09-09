using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.Movies.Queries.GetList
{
    public class GetMovieListQuery : IRequest<GetListResponse<GetMoviesListDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest pageRequest;

        public string? CacheKey => $"GetMovieListQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public string? CacheGroupKey => "Movies";

        public bool ByPassCache { get; }

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetMovieListHandler : IRequestHandler<GetMovieListQuery, GetListResponse<GetMoviesListDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieListHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetMoviesListDto>> Handle(GetMovieListQuery request, CancellationToken cancellationToken)
        {
            var list = await _movieRepository.GetListAsync(
                predicate: null,
                include: m => m.Include(x => x.Publisher),
                index: request.pageRequest.PageIndex,
                size: request.pageRequest.PageSize,
                withDeleted: false,
                cancellationToken: cancellationToken);

            GetListResponse<GetMoviesListDto> getMoviesListDto = _mapper.Map<GetListResponse<GetMoviesListDto>>(list);

            return getMoviesListDto;

        }
    }
}
