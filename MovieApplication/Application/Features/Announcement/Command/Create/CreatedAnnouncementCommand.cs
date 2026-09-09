using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Announcement.Command.Create
{
    public class CreatedAnnouncementCommand : IRequest<CreatedAnnouncementResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subtitle1 { get; set; }
        public string Subtitle2 { get; set; }

        public string? CacheKey => $"CreatedAnnouncementCommand Title:{Title}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Announcements";
    }

    public class CreatedAnnouncementHandler : IRequestHandler<CreatedAnnouncementCommand, CreatedAnnouncementResponse>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public CreatedAnnouncementHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task<CreatedAnnouncementResponse> Handle(CreatedAnnouncementCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Announcement? announcement = _mapper.Map<Domain.Entities.Announcement>(request);

            var entity = await _announcementRepository.AddAsync(announcement);

            var result = _mapper.Map<CreatedAnnouncementResponse>(entity);

            return result;
        }
    }
}
