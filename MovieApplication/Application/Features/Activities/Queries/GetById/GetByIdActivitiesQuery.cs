using Application.Features.Activities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Activities.Queries.GetById
{
    public class GetByIdActivitiesQuery : IRequest<GetByIdActivitiesDto>, ILoggableRequest
    {
        public Guid Id { get; set; }
    }

    public class GetByIdActivitiesQueryHandler : IRequestHandler<GetByIdActivitiesQuery, GetByIdActivitiesDto>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IMapper _mapper;
        private readonly ActivitiesBusinessRules _activitiesBusinessRules;

        public GetByIdActivitiesQueryHandler(IActivitiesRepository activitiesRepository, IMapper mapper, ActivitiesBusinessRules activitiesBusinessRules)
        {
            _activitiesRepository = activitiesRepository;
            _mapper = mapper;
            _activitiesBusinessRules = activitiesBusinessRules;
        }

        public async Task<GetByIdActivitiesDto> Handle(GetByIdActivitiesQuery request, CancellationToken cancellationToken)
        {
            await _activitiesBusinessRules.NoActivityFound(request.Id);

            Domain.Entities.Activities? activity = await _activitiesRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            GetByIdActivitiesDto result = _mapper.Map<GetByIdActivitiesDto>(activity);

            return result;
        }
    }
}
