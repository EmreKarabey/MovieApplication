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
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.LikedMovie.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.LikedMovie, CreatedLikedMovieCommand>().ReverseMap();
            CreateMap<Domain.Entities.LikedMovie, CreatedLikedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.LikedMovie, DeletedLikedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.LikedMovie, UpdateLikedMovieResponse>().ReverseMap();
            CreateMap<Domain.Entities.LikedMovie, UpdateLikedMovieCommand>().ReverseMap();

            CreateMap<Domain.Entities.LikedMovie, GetByIdLikedMovieDto>().ReverseMap();


            CreateMap<Domain.Entities.LikedMovie, GetListLikedMovieDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.LikedMovie>, GetListResponse<GetListLikedMovieDto>>().ReverseMap();
        }
    }
}
