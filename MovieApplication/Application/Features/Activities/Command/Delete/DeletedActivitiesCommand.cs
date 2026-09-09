using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Activities.Rules;
using Application.Features.FavoriteMovie.Command.Delete;
using Application.Features.FavoriteMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.Activities.Command.Delete
{
    public class DeletedActivitiesCommand : IRequest<DeletedActivitiesResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"DeletedActivitiesCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Activities";
    }

    public class DeletedActivitiesHandler : IRequestHandler<DeletedActivitiesCommand, DeletedActivitiesResponse>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IMapper _mapper;
        private readonly ActivitiesBusinessRules _activitiesBusinessRules;

        public DeletedActivitiesHandler(IActivitiesRepository activitiesRepository, IMapper mapper, ActivitiesBusinessRules activitiesBusinessRules)
        {
            _activitiesRepository = activitiesRepository;
            _mapper = mapper;
            _activitiesBusinessRules = activitiesBusinessRules;
        }

        public async Task<DeletedActivitiesResponse> Handle(DeletedActivitiesCommand request, CancellationToken cancellationToken)
        {
            await _activitiesBusinessRules.NoActivityFound(request.Id);

            Domain.Entities.Activities activities = await _activitiesRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteActivity = await _activitiesRepository.DeleteAsync(activities, permanent: false);

            var result = _mapper.Map<DeletedActivitiesResponse>(deleteActivity);

            return result;
        }
    }
}
