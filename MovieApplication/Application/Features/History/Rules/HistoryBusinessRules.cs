using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Constants;
using Application.Features.LikedMovie.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CorePersistence.Paginate;
using Microsoft.AspNetCore.Http;

namespace Application.Features.History.Rules
{
    public class HistoryBusinessRules
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HistoryBusinessRules(IHistoryRepository historyRepository, IHttpContextAccessor httpContextAccessor)
        {
            _historyRepository = historyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoHistoryFound(Guid Id)
        {
            Domain.Entities.History history = await _historyRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (history == null) throw new BusinessException(HistoryMessages.NoHistoryFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);


            Domain.Entities.History history = await _historyRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (history.UserID != currentUserId) throw new BusinessException(HistoryMessages.NotBeVerified);
        }

        public async Task<Domain.Entities.History?> GetAndClearOldHistoryAsync(Guid MovieId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);

            Paginate<Domain.Entities.History> history = await _historyRepository.GetListAsync(predicate: n => n.MovieID == MovieId && n.UserID == currentUserId);

            if (history.Items.Any())
            {
                var oldRecord = history.Items.First();
                await _historyRepository.DeleteRangeAsync(history.Items);
                return oldRecord;
            }
            return null;
        }

    }
}
