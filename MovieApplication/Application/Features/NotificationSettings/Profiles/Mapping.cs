using System;
using Application.Features.NotificationSettings.Command.ChangeSettings;
using Application.Features.NotificationSettings.Command.Create;
using Application.Features.NotificationSettings.Command.Delete;
using Application.Features.NotificationSettings.Command.Update;
using Application.Features.NotificationSettings.Queries.GetById;
using Application.Features.NotificationSettings.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.NotificationSettings.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.NotificationSettings, CreatedNotificationSettingsCommand>().ReverseMap();
            CreateMap<Domain.Entities.NotificationSettings, CreatedNotificationSettingsResponse>().ReverseMap();

            CreateMap<Domain.Entities.NotificationSettings, DeletedNotificationSettingsCommand>().ReverseMap();
            CreateMap<Domain.Entities.NotificationSettings, DeletedNotificationSettingsResponse>().ReverseMap();

            CreateMap<Domain.Entities.NotificationSettings, UpdatedNotificationSettingsCommand>().ReverseMap();
            CreateMap<Domain.Entities.NotificationSettings, UpdatedNotificationSettingsResponse>().ReverseMap();

            CreateMap<Domain.Entities.NotificationSettings, GetByIdNotificationSettingsDto>().ReverseMap();

            CreateMap<Domain.Entities.NotificationSettings, GetListNotificationSettingsDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.NotificationSettings>, GetListResponse<GetListNotificationSettingsDto>>().ReverseMap();

            CreateMap<Domain.Entities.NotificationSettings, ChangeNotificationSettingsCommand>().ReverseMap();
            CreateMap<Domain.Entities.NotificationSettings, ChangeNotificationSettingsResponse>().ReverseMap();
        }
    }
}
