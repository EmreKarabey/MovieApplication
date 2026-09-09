using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Rules;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.History.Command.Delete
{
    public class DeletedHistoryCommand : IRequest<DeletedHistoryResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedHistoryCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Histories";
    }
    public class DeletedHistoryHandler : IRequestHandler<DeletedHistoryCommand, DeletedHistoryResponse>
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly HistoryBusinessRules _historyBusinessRules;

        public DeletedHistoryHandler(IMapper mapper, IHistoryRepository historyRepository, HistoryBusinessRules historyBusinessRules)
        {
            _mapper = mapper;
            _historyRepository = historyRepository;
            _historyBusinessRules = historyBusinessRules;
        }

        public async Task<DeletedHistoryResponse> Handle(DeletedHistoryCommand request, CancellationToken cancellationToken)
        {
            await _historyBusinessRules.NoHistoryFound(request.Id);

            Domain.Entities.History history = await _historyRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteLikedMovie = await _historyRepository.DeleteAsync(history, permanent: false);

            var result = _mapper.Map<DeletedHistoryResponse>(deleteLikedMovie);

            return result;
        }
    }
}
