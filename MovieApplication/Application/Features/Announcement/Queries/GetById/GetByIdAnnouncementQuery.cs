using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Rules;
using Application.Features.Comment.Queries.GetById;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.Announcement.Queries.GetById
{
    public class GetByIdAnnouncementQuery : IRequest<GetByIdAnnouncementDto>, ILoggableRequest, ICachableRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"GetByIdAnnouncementQuery Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Announcements";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdAnnouncementHandler : IRequestHandler<GetByIdAnnouncementQuery, GetByIdAnnouncementDto>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly AnnouncementBusinessRules _announcementBusinessRules;

        public GetByIdAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper, AnnouncementBusinessRules announcementBusinessRules)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            _announcementBusinessRules = announcementBusinessRules;
        }

        public async Task<GetByIdAnnouncementDto> Handle(GetByIdAnnouncementQuery request, CancellationToken cancellationToken)
        {
            await _announcementBusinessRules.NoAnnouncementFound(request.Id);

            var entity = await _announcementRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var result = _mapper.Map<GetByIdAnnouncementDto>(entity);
            return result;
        }
    }
}
