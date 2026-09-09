using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.History.Command.Update
{
    public class UpdateHistoryCommand : IRequest<UpdateHistoryResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }

        public Guid MovieID { get; set; }

        public string? CacheKey => $"UpdateHistoryCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";
    }

    public class UpdateHistoryHandler : IRequestHandler<UpdateHistoryCommand, UpdateHistoryResponse>
    {
        private readonly HistoryBusinessRules _historyBusinessRules;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateHistoryHandler(HistoryBusinessRules historyBusinessRules, IHistoryRepository historyRepository, IMapper mapper)
        {
            _historyBusinessRules = historyBusinessRules;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<UpdateHistoryResponse> Handle(UpdateHistoryCommand request, CancellationToken cancellationToken)
        {

            await _historyBusinessRules.NoHistoryFound(request.Id);

            await _historyBusinessRules.UserAuthentication(request.Id);

            Domain.Entities.History history = await _historyRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            history = _mapper.Map(request, history);

            var updateHistory = await _historyRepository.UpdateAsync(history);

            var result = _mapper.Map<UpdateHistoryResponse>(updateHistory);

            return result;
        }
    }
}
