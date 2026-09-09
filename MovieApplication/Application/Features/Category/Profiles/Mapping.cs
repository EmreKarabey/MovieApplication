using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Command.Create;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Queries.GetById;
using Application.Features.Category.Queries.GetList;
using Application.Features.Movies.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.Category.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Category, CreatedCategoryCommand>().ReverseMap();
            CreateMap<Domain.Entities.Category, CreatedCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.Category, DeletedCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.Category, UpdatedCategoryResponse>().ReverseMap();
            CreateMap<Domain.Entities.Category, UpdatedCategoryCommand>().ReverseMap();

            CreateMap<Domain.Entities.Category, GetByIdCategoryDto>().ReverseMap();
            CreateMap<Domain.Entities.Category, GetListCategoryDto>().ReverseMap();

            CreateMap<Paginate<Domain.Entities.Category>, GetListResponse<GetByIdCategoryDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Category>, GetListResponse<GetListCategoryDto>>().ReverseMap();
        }
    }
}
