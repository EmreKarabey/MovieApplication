using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.History.Command.Create
{
    public class CreatedHistoryCommand : IRequest<CreatedHistoryResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid MovieID { get; set; }
        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }

        public string? CacheKey => $"CreatedHistoryCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";
    }

    public class CreatedHistoryHandler : IRequestHandler<CreatedHistoryCommand, CreatedHistoryResponse>
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly HistoryBusinessRules _historyBusinessRules;

        public CreatedHistoryHandler(IHistoryRepository historyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules, HistoryBusinessRules historyBusinessRules)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
            _historyBusinessRules = historyBusinessRules;
        }

        public async Task<CreatedHistoryResponse> Handle(CreatedHistoryCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);
            var oldHistory = await _historyBusinessRules.GetAndClearOldHistoryAsync(request.MovieID);

            var entity = _mapper.Map<Domain.Entities.History>(request);

            if (oldHistory != null && request.TotalSeconds == 0 && request.WatchedSeconds == 0)
            {
                entity.TotalSeconds = oldHistory.TotalSeconds;
                entity.WatchedSeconds = oldHistory.WatchedSeconds;
            }

            entity.UserID = currentUserId;

            var addlikedMovie = await _historyRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedHistoryResponse>(addlikedMovie);

            return result;
        }
    }
}
