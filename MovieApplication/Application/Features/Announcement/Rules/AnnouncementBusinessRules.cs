using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Constants;
using Application.Features.Comment.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;

namespace Application.Features.Announcement.Rules
{
    public class AnnouncementBusinessRules : BaseBusinessRules
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementBusinessRules(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task NoAnnouncementFound(Guid Id)
        {
            Domain.Entities.Announcement announcement = await _announcementRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (announcement == null) throw new BusinessException(AnnouncementMessages.NoAnnouncementFound);
        }
    }
}
