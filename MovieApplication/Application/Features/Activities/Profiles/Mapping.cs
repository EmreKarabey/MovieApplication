using Application.Features.Activities.Command.Create;
using Application.Features.Activities.Command.Delete;
using Application.Features.Activities.Command.Update;
using Application.Features.Activities.Queries.GetById;
using Application.Features.Activities.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.Activities.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Activities, CreatedActivitiesCommand>().ReverseMap();
            CreateMap<Domain.Entities.Activities, CreatedActivitiesResponse>().ReverseMap();

            CreateMap<Domain.Entities.Activities, DeletedActivitiesResponse>().ReverseMap();

            CreateMap<Domain.Entities.Activities, UpdateActivitiesResponse>().ReverseMap();
            CreateMap<Domain.Entities.Activities, UpdateActivitiesCommand>().ReverseMap();

            CreateMap<Domain.Entities.Activities, GetByIdActivitiesDto>().ReverseMap();
            CreateMap<Domain.Entities.Activities, GetListActivitiesDto>().ReverseMap();

            CreateMap<Paginate<Domain.Entities.Activities>, GetListResponse<GetByIdActivitiesDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Activities>, GetListResponse<GetListActivitiesDto>>().ReverseMap();
        }
    }
}
