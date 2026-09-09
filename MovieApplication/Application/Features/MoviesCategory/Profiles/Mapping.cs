using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Create;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Queries.GetById;
using Application.Features.Comment.Queries.GetList;
using Application.Features.MoviesCategory.Commands.Create;
using Application.Features.MoviesCategory.Commands.Delete;
using Application.Features.MoviesCategory.Commands.Update;
using Application.Features.MoviesCategory.Queries.GetById;
using Application.Features.MoviesCategory.Queries.GetCategoryName;
using Application.Features.MoviesCategory.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.MoviesCategory.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.MoviesCategory, CreatedMoviesCategoryCommand>().ReverseMap();
            CreateMap<Domain.Entities.MoviesCategory, CreatedMoviesCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.MoviesCategory, DeletedMovieCategoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.MoviesCategory, UpdatedMovieCategoryResponse>().ReverseMap();
            CreateMap<Domain.Entities.MoviesCategory, UpdatedMovieCategoryCommand>().ReverseMap();

            CreateMap<Domain.Entities.MoviesCategory, GetByIdMovieCategoryDto>().ReverseMap();

            CreateMap<Domain.Entities.MoviesCategory, GetListMovieCategoryDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.MoviesCategory>, GetListResponse<GetListMovieCategoryDto>>().ReverseMap();
        }
    }
}
