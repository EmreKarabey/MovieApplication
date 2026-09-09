using Application.Features.Activities.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Activities.Command.Update
{
    public class UpdateActivitiesCommand : IRequest<UpdateActivitiesResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }
        public ActivitiesCategory ActivitiesCategory { get; set; }

        public string? CacheKey => $"UpdateActivitiesCommand Id:{Id}";
        public bool ByPassCache { get; }
        public string? CacheGroupKey => "Activities";
    }

    public class UpdateActivitiesHandler : IRequestHandler<UpdateActivitiesCommand, UpdateActivitiesResponse>
    {
        private readonly ActivitiesBusinessRules _activitiesBusinessRules;
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IMapper _mapper;

        public UpdateActivitiesHandler(IActivitiesRepository activitiesRepository, IMapper mapper, ActivitiesBusinessRules activitiesBusinessRules)
        {
            _activitiesRepository = activitiesRepository;
            _mapper = mapper;
            _activitiesBusinessRules = activitiesBusinessRules;
        }

        public async Task<UpdateActivitiesResponse> Handle(UpdateActivitiesCommand request, CancellationToken cancellationToken)
        {
            await _activitiesBusinessRules.NoActivityFound(request.Id);

            Domain.Entities.Activities? activity = await _activitiesRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            activity = _mapper.Map(request, activity);

            var updatedEntity = await _activitiesRepository.UpdateAsync(activity);

            var result = _mapper.Map<UpdateActivitiesResponse>(updatedEntity);

            return result;
        }
    }
}
