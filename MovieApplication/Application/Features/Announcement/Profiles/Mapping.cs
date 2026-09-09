using Application.Features.Announcement.Command.Create;
using Application.Features.Announcement.Command.Delete;
using Application.Features.Announcement.Command.Update;
using Application.Features.Announcement.Queries.GetById;
using Application.Features.Announcement.Queries.GetLİst;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.Announcement.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Announcement, CreatedAnnouncementCommand>().ReverseMap();
            CreateMap<Domain.Entities.Announcement, CreatedAnnouncementResponse>().ReverseMap();

            CreateMap<Domain.Entities.Announcement, DeletedAnnouncementResponse>().ReverseMap();

            CreateMap<Domain.Entities.Announcement, UpdateAnnouncementResponse>().ReverseMap();
            CreateMap<Domain.Entities.Announcement, UpdateAnnouncementCommand>().ReverseMap();

            CreateMap<Domain.Entities.Announcement, GetByIdAnnouncementDto>().ReverseMap();
            CreateMap<Domain.Entities.Announcement, GetListAnnouncementDto>().ReverseMap();

            CreateMap<Domain.Entities.Announcement, GetListResponse<GetListAnnouncementDto>>().ReverseMap();
        }
    }
}
