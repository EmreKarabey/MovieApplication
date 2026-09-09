using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ForumCategory.Command.Create;
using Application.Features.ForumCategory.Command.Delete;
using Application.Features.ForumCategory.Command.Update;
using Application.Features.ForumCategory.Queries.GetById;
using Application.Features.ForumCategory.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.ForumCategory.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.ForumCategory, CreatedForumCategoryCommand>().ReverseMap();
            CreateMap<Domain.Entities.ForumCategory, CreatedForumCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.ForumCategory, DeletedForumCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.ForumCategory, UpdatedForumCategoryResponse>().ReverseMap();
            CreateMap<Domain.Entities.ForumCategory, UpdatedForumCategoryCommand>().ReverseMap();

            CreateMap<Domain.Entities.ForumCategory, GetByIdForumCategoryDto>().ReverseMap();
            CreateMap<Domain.Entities.ForumCategory, GetListForumCategoryDto>().ReverseMap();

            CreateMap<Paginate<Domain.Entities.ForumCategory>, GetListResponse<GetByIdForumCategoryDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.ForumCategory>, GetListResponse<GetListForumCategoryDto>>().ReverseMap();
        }
    }
}
