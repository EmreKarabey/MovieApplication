using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Command.Create;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Activities.Command.Create
{
    public class CreatedActivitiesCommand : IRequest<CreatedActivitiesResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest
    {
        public Guid MovieID { get; set; }

        public int ActivitiesCategory { get; set; }

        public string? CacheKey => "CreatedActivitiesCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Activities";

    }

    public class CreatedActivitiesHandler : IRequestHandler<CreatedActivitiesCommand, CreatedActivitiesResponse>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IMapper _mapper;

        public CreatedActivitiesHandler(IActivitiesRepository activitiesRepository, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules, IMapper mapper)
        {
            _activitiesRepository = activitiesRepository;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
            _mapper = mapper;
        }

        public async Task<CreatedActivitiesResponse> Handle(CreatedActivitiesCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.Activities>(request);

            entity.UserID = currentUserId;

            var result = await _activitiesRepository.AddAsync(entity);

            return _mapper.Map<CreatedActivitiesResponse>(result); ;
        }
    }
}
