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

namespace Application.Features.UnlikedMovie.Queries.GetList
{
    public class GetListUnlikedMovieQuery : IRequest<GetListResponse<GetListUnlikedMovieDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListUnlikedMovieQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "UnlikedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListUnlikedMovieHandler : IRequestHandler<GetListUnlikedMovieQuery, GetListResponse<GetListUnlikedMovieDto>>
    {
        private readonly IUnlikedMovieRepository _unlikedMovieRepository;
        private readonly IMapper _mapper;
        private readonly LikedMovieBusinessRules _likedMovieBusinessRules;

        public GetListUnlikedMovieHandler(IUnlikedMovieRepository unlikedMovieRepository, IMapper mapper, LikedMovieBusinessRules likedMovieBusinessRules)
        {
            _unlikedMovieRepository = unlikedMovieRepository;
            _mapper = mapper;
            _likedMovieBusinessRules = likedMovieBusinessRules;
        }

        public async Task<GetListResponse<GetListUnlikedMovieDto>> Handle(GetListUnlikedMovieQuery request, CancellationToken cancellationToken)
        {
            var list = await _unlikedMovieRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null);

            var result = _mapper.Map<GetListResponse<GetListUnlikedMovieDto>>(list);

            return result;
        }
    }
}
