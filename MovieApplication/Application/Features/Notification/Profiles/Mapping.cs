using System;
using Application.Features.Notification.Command.Create;
using Application.Features.Notification.Command.Delete;
using Application.Features.Notification.Command.Update;
using Application.Features.Notification.Queries.GetById;
using Application.Features.Notification.Queries.GetList;
using Application.Features.Notification.Queries.GetMyNotificationList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.Notification.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Notification, CreatedNotificationCommand>().ReverseMap();
            CreateMap<Domain.Entities.Notification, CreatedNotificationResponse>().ReverseMap();

            CreateMap<Domain.Entities.Notification, DeletedNotificationCommand>().ReverseMap();
            CreateMap<Domain.Entities.Notification, DeletedNotificationResponse>().ReverseMap();

            CreateMap<Domain.Entities.Notification, UpdateNotificationCommand>().ReverseMap();
            CreateMap<Domain.Entities.Notification, UpdateNotificationResponse>().ReverseMap();

            CreateMap<Domain.Entities.Notification, GetByIdNotificationDto>().ReverseMap();

            CreateMap<Domain.Entities.Notification, GetListNotificationDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Notification>, GetListResponse<GetListNotificationDto>>().ReverseMap();

            CreateMap<Domain.Entities.Notification, GetMyNotificationListDto>().ReverseMap();
        }
    }
}
