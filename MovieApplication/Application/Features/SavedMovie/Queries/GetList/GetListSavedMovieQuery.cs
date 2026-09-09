using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Rules;
using Application.Features.SavedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.SavedMovie.Queries.GetList
{
    public class GetListSavedMovieQuery : IRequest<GetListResponse<GetListSavedMovieDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListSavedMovieQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SavedMovies";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListSavedMovieHandler : IRequestHandler<GetListSavedMovieQuery, GetListResponse<GetListSavedMovieDto>>
    {
        private readonly ISavedMovieRepository _savedMovieRepository;
        private readonly IMapper _mapper;
        private readonly SavedMovieBusinessRules _savedMovieBusinessRules;

        public GetListSavedMovieHandler(ISavedMovieRepository savedMovieRepository, IMapper mapper, SavedMovieBusinessRules savedMovieBusinessRules)
        {
            _savedMovieRepository = savedMovieRepository;
            _mapper = mapper;
            _savedMovieBusinessRules = savedMovieBusinessRules;
        }

        public async Task<GetListResponse<GetListSavedMovieDto>> Handle(GetListSavedMovieQuery request, CancellationToken cancellationToken)
        {
            var list = await _savedMovieRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null);

            var result = _mapper.Map<GetListResponse<GetListSavedMovieDto>>(list);

            return result;
        }
    }
}
