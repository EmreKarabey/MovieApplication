using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.HelsinkiService;
using Application.Services.Repositories;
using Application.Services.ToxicBertService;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Forum.Command.Create
{
    public class CreatedForumCommand : IRequest<CreatedForumResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public Guid ForumCategoryId { get; set; }

        public string? CacheKey => $"CreatedForumCommand Title:{Title}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Forums";
    }

    public class CreatedForumHandler : IRequestHandler<CreatedForumCommand, CreatedForumResponse>
    {
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IForumRepository _forumRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToxicBertService _toxicBertService;
        private readonly IHelsinkiService _helsinkiService;

        public CreatedForumHandler(IForumRepository forumRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules, IToxicBertService toxicBertService, IHelsinkiService helsinkiService)
        {
            _forumRepository = forumRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
            _toxicBertService = toxicBertService;
            _helsinkiService = helsinkiService;
        }

        public async Task<CreatedForumResponse> Handle(CreatedForumCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User
     .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.Forum>(request);

            entity.UserId = currentUserId;

            entity.CreatedAt = DateTime.UtcNow;

            var translateTitle = _helsinkiService.Translate(request.Title);
            var translateDetails = _helsinkiService.Translate(request.Details);

            var taskAll1 = await Task.WhenAll(translateTitle, translateDetails);

            var EnTitle = translateTitle.Result;
            var EnDetails = translateDetails.Result;

            var textscore1 = _toxicBertService.ToxicScore(EnTitle);
            var textscore2 = _toxicBertService.ToxicScore(EnDetails);

            var taskAll2 = await Task.WhenAll(textscore1, textscore2);

            var TextScore1 = textscore1.Result;
            var TextScore2 = textscore2.Result;

            bool IsToxic(List<ToxicScoreDto> toxicScoreDtos) => toxicScoreDtos.Any(s => s.Score > 0.5);

            if (IsToxic(TextScore1) || IsToxic(TextScore2)) entity.Status = "Toksik Yazý";

            if (string.IsNullOrEmpty(entity.Status)) entity.Status = "Onaylandý";

            var addForum = await _forumRepository.AddAsync(entity);

            var result = new CreatedForumResponse
            {
                Status = addForum.Status,
                Details = addForum.Details,
                EntityID = addForum.EntityID,
                ForumCategoryId = addForum.ForumCategoryId,
                Title = addForum.Title,
                UserId = addForum.UserId
            };

            return result;
        }
    }
}
