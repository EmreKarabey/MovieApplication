using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Forum.Command.Create;
using Application.Features.Forum.Command.Delete;
using Application.Features.Forum.Command.Update;
using Application.Features.Forum.Queries.GetById;
using Application.Features.Forum.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.Forum.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Forum, CreatedForumCommand>().ReverseMap();
            CreateMap<Domain.Entities.Forum, CreatedForumResponse>().ReverseMap();

            CreateMap<Domain.Entities.Forum, DeletedForumResponse>().ReverseMap();

            CreateMap<Domain.Entities.Forum, UpdateForumResponse>().ReverseMap();
            CreateMap<Domain.Entities.Forum, UpdateForumCommand>().ReverseMap();

            CreateMap<Domain.Entities.Forum, GetByIdForumDto>().ReverseMap();

            CreateMap<Domain.Entities.Forum, GetListForumDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Forum>, GetListResponse<GetListForumDto>>().ReverseMap();
        }
    }
}
