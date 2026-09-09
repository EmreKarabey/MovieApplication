using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Activities.Queries.GetList
{
    public class GetListActivitiesQuery : IRequest<GetListResponse<GetListActivitiesDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest PageRequest { get; set; }

        public string CacheKey => $"GetListActivitiesQuery({PageRequest.PageIndex},{PageRequest.PageSize})";
        public bool ByPassCache { get; }
        public string? CacheGroupKey => "Activities";
        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetListActivitiesQueryHandler : IRequestHandler<GetListActivitiesQuery, GetListResponse<GetListActivitiesDto>>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IMapper _mapper;

        public GetListActivitiesQueryHandler(IActivitiesRepository activitiesRepository, IMapper mapper)
        {
            _activitiesRepository = activitiesRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListActivitiesDto>> Handle(GetListActivitiesQuery request, CancellationToken cancellationToken)
        {
            Paginate<Domain.Entities.Activities> activities = await _activitiesRepository.GetListAsync(
                predicate: null,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListActivitiesDto> result = _mapper.Map<GetListResponse<GetListActivitiesDto>>(activities);

            return result;
        }
    }
}
