using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetList;
using Application.Features.Comment.Rules;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.LikedMovie.Queries.GetList
{
    public class GetListLikedMovieQuery : IRequest<GetListResponse<GetListLikedMovieDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListLikedMovieQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "LikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListLikedMovieHandler : IRequestHandler<GetListLikedMovieQuery, GetListResponse<GetListLikedMovieDto>>
    {
        private readonly ILikedMovieRepository _likedMovieRepository;
        private readonly IMapper _mapper;
        private readonly LikedMovieBusinessRules _likedMovieBusinessRules;

        public GetListLikedMovieHandler(ILikedMovieRepository likedMovieRepository, IMapper mapper, LikedMovieBusinessRules likedMovieBusinessRules)
        {
            _likedMovieRepository = likedMovieRepository;
            _mapper = mapper;
            _likedMovieBusinessRules = likedMovieBusinessRules;
        }

        public async Task<GetListResponse<GetListLikedMovieDto>> Handle(GetListLikedMovieQuery request, CancellationToken cancellationToken)
        {
            var list = await _likedMovieRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null);

            var result = _mapper.Map<GetListResponse<GetListLikedMovieDto>>(list);

            return result;
        }
    }
}
