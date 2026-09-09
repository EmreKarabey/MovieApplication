using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Rules;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Announcement.Command.Update
{
    public class UpdateAnnouncementCommand : IRequest<UpdateAnnouncementResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subtitle1 { get; set; }
        public string Subtitle2 { get; set; }

        public string? CacheKey => $"UpdateAnnouncementCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Announcements";
    }

    public class UpdateAnnouncementHandler : IRequestHandler<UpdateAnnouncementCommand, UpdateAnnouncementResponse>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly AnnouncementBusinessRules _announcementBusinessRules;
        private readonly IMapper _mapper;

        public UpdateAnnouncementHandler(IAnnouncementRepository announcementRepository, AnnouncementBusinessRules announcementBusinessRules, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _announcementBusinessRules = announcementBusinessRules;
            _mapper = mapper;
        }

        public async Task<UpdateAnnouncementResponse> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            await _announcementBusinessRules.NoAnnouncementFound(request.Id);

            Domain.Entities.Announcement announcement = await _announcementRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            announcement = _mapper.Map(request, announcement);

            var updateAnnouncement = await _announcementRepository.UpdateAsync(announcement);

            var result = _mapper.Map<UpdateAnnouncementResponse>(updateAnnouncement);

            return result;
        }
    }
}
