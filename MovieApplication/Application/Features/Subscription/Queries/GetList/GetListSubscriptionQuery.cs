using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Subscription.Queries.GetList
{
    public class GetListSubscriptionQuery : IRequest<GetListResponse<GetListSubscriptionDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest pageRequest { get; set; }

        public string? CacheKey => $"GetListSubscriptionQuery({pageRequest?.PageIndex},{pageRequest?.PageSize})";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Subcription";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListSubscriptionQueryHandler : IRequestHandler<GetListSubscriptionQuery, GetListResponse<GetListSubscriptionDto>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public GetListSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListSubscriptionDto>> Handle(GetListSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var list = await _subscriptionRepository.GetListAsync(predicate: null, index: request.pageRequest.PageIndex, size: request.pageRequest.PageSize);
            var result = _mapper.Map<GetListResponse<GetListSubscriptionDto>>(list);
            return result;
        }
    }
}
