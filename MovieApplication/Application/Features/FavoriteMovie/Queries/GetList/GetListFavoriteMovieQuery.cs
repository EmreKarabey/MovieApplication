using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Rules;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.FavoriteMovie.Queries.GetList
{
    public class GetListFavoriteMovieQuery : IRequest<GetListResponse<GetListFavoriteMovieDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListFavoriteMovieQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "FavoriteMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListFavoriteMovieHandler : IRequestHandler<GetListFavoriteMovieQuery, GetListResponse<GetListFavoriteMovieDto>>
    {
        private readonly IFavoriteMovieRepository _favoriteMovieRepository;
        private readonly IMapper _mapper;
        private readonly FavoriteMovieBusinessRules _favoriteMovieBusinessRules;

        public GetListFavoriteMovieHandler(IFavoriteMovieRepository favoriteMovieRepository, IMapper mapper, FavoriteMovieBusinessRules favoriteMovieBusinessRules)
        {
            _favoriteMovieRepository = favoriteMovieRepository;
            _mapper = mapper;
            _favoriteMovieBusinessRules = favoriteMovieBusinessRules;
        }

        public async Task<GetListResponse<GetListFavoriteMovieDto>> Handle(GetListFavoriteMovieQuery request, CancellationToken cancellationToken)
        {
            var list = await _favoriteMovieRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null);

            var result = _mapper.Map<GetListResponse<GetListFavoriteMovieDto>>(list);

            return result;
        }
    }
}
