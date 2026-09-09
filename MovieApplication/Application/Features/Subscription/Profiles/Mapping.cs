using Application.Features.Subscription.Command.Create;
using Application.Features.Subscription.Command.Delete;
using Application.Features.Subscription.Command.Update;
using Application.Features.Subscription.Queries.GetById;
using Application.Features.Subscription.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.Subscription.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Subscription, CreatedSubscriptionCommand>().ReverseMap();
            CreateMap<Domain.Entities.Subscription, CreatedSubscriptionResponse>().ReverseMap();

            CreateMap<Domain.Entities.Subscription, UpdatedSubscriptionCommand>().ReverseMap();
            CreateMap<Domain.Entities.Subscription, UpdatedSubscriptionResponse>().ReverseMap();

            CreateMap<Domain.Entities.Subscription, DeletedSubscriptionResponse>().ReverseMap();

            CreateMap<Domain.Entities.Subscription, GetByIdSubscriptionDto>().ReverseMap();
            CreateMap<Domain.Entities.Subscription, GetListSubscriptionDto>().ReverseMap();

            CreateMap<Paginate<Domain.Entities.Subscription>, GetListResponse<GetByIdSubscriptionDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Subscription>, GetListResponse<GetListSubscriptionDto>>().ReverseMap();
        }
    }
}
