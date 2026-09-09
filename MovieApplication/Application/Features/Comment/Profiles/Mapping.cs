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
using Application.Features.Comment.Command.Create;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Queries.GetById;
using Application.Features.Comment.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.Comment.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Comments, CreatedCommentCommand>().ReverseMap();
            CreateMap<Comments, CreatedCommentResponse>().ReverseMap();

            CreateMap<Comments, DeletedCommentResponse>().ReverseMap();

            CreateMap<Comments, UpdateCommentResponse>().ReverseMap();
            CreateMap<Comments, UpdateCommentCommand>().ReverseMap();

            CreateMap<Comments, GetByIdCommentDto>().ReverseMap();


            CreateMap<Comments, GetListCommentDto>().ReverseMap();
            CreateMap<Paginate<Comments>, GetListResponse<GetListCommentDto>>().ReverseMap();
        }
    }
}
