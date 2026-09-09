using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;

namespace Application.Features.Announcement.Command.Delete
{
    public class DeletedAnnouncementCommand : IRequest<DeletedAnnouncementResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }
        public string? CacheKey => $"DeletedAnnouncementCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Announcements";
    }
    public class DeletedAnnouncementHandler : IRequestHandler<DeletedAnnouncementCommand, DeletedAnnouncementResponse>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly AnnouncementBusinessRules _announcementBusinessRules;
        private readonly IMapper _mapper;

        public DeletedAnnouncementHandler(IAnnouncementRepository announcementRepository, AnnouncementBusinessRules announcementBusinessRules, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _announcementBusinessRules = announcementBusinessRules;
            _mapper = mapper;
        }

        public async Task<DeletedAnnouncementResponse> Handle(DeletedAnnouncementCommand request, CancellationToken cancellationToken)
        {
            await _announcementBusinessRules.NoAnnouncementFound(request.Id);

            var entity = await _announcementRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteEntity = await _announcementRepository.DeleteAsync(entity);

            var result = _mapper.Map<DeletedAnnouncementResponse>(deleteEntity);

            return result;
        }
    }
}
