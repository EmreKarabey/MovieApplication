using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;

namespace Application.Features.History.Queries.GetList
{
    public class GetListHistoryQuery : IRequest<GetListResponse<GetListHistoryDto>>, ILoggableRequest, ICachableRequest
    {
        public PageRequest pageRequest;
        public string? CacheKey => $"GetListHistoryQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListLikedMovieHandler : IRequestHandler<GetListHistoryQuery, GetListResponse<GetListHistoryDto>>
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly HistoryBusinessRules _historyBusinessRules;

        public GetListLikedMovieHandler(IHistoryRepository historyRepository, IMapper mapper, HistoryBusinessRules historyBusinessRules)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
            _historyBusinessRules = historyBusinessRules;
        }

        public async Task<GetListResponse<GetListHistoryDto>> Handle(GetListHistoryQuery request, CancellationToken cancellationToken)
        {
            var list = await _historyRepository.GetListAsync(index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize, predicate: null);

            var result = _mapper.Map<GetListResponse<GetListHistoryDto>>(list);

            return result;
        }
    }
}
