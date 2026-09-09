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
using Application.Features.SubComments.Command.Create;
using Application.Features.SubComments.Command.Delete;
using Application.Features.SubComments.Command.Update;
using Application.Features.SubComments.Queries.GetById;
using Application.Features.SubComments.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.SubComments.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SubComment, CreatedSubCommentCommand>().ReverseMap();
            CreateMap<SubComment, CreatedSubCommentResponse>().ReverseMap();

            CreateMap<SubComment, DeletedSubCommentResponse>().ReverseMap();

            CreateMap<SubComment, UpdateSubCommentResponse>().ReverseMap();
            CreateMap<SubComment, UpdateSubCommentCommand>().ReverseMap();

            CreateMap<SubComment, GetByIdSubCommentDto>().ReverseMap();


            CreateMap<SubComment, GetListSubCommentDto>().ReverseMap();
            CreateMap<Paginate<SubComment>, GetListResponse<GetListSubCommentDto>>().ReverseMap();
        }
    }
}




