using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.History.Queries.GetById
{
    public class GetByIdHistoryQuery : IRequest<GetByIdHistoryDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdHistoryQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdHistoryHandler : IRequestHandler<GetByIdHistoryQuery, GetByIdHistoryDto>
    {
        private readonly HistoryBusinessRules _historyBusinessRules;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public GetByIdHistoryHandler(HistoryBusinessRules historyBusinessRules, IHistoryRepository historyRepository, IMapper mapper)
        {
            _historyBusinessRules = historyBusinessRules;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdHistoryDto> Handle(GetByIdHistoryQuery request, CancellationToken cancellationToken)
        {
            await _historyBusinessRules.NoHistoryFound(request.Id);
            var entity = await _historyRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdHistoryDto>(entity);

            return result;
        }
    }
}
