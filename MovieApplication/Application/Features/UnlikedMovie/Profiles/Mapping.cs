using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.UnlikedMovie.Command.Delete;
using Application.Features.UnlikedMovie.Commands.Create;
using Application.Features.UnlikedMovie.Commands.Update;
using Application.Features.UnlikedMovie.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.UnlikedMovie.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.UnlikedMovie, CreatedUnlikedMovieCommand>().ReverseMap();
            CreateMap<Domain.Entities.UnlikedMovie, CreatedUnlikedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.UnlikedMovie, DeletedUnlikedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.UnlikedMovie, UpdateUnlikedMovieResponse>().ReverseMap();
            CreateMap<Domain.Entities.UnlikedMovie, UpdateUnlikedMovieCommand>().ReverseMap();

            CreateMap<Domain.Entities.UnlikedMovie, GetByIdLikedMovieDto>().ReverseMap();


            CreateMap<Domain.Entities.UnlikedMovie, GetListUnlikedMovieDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.UnlikedMovie>, GetListResponse<GetListUnlikedMovieDto>>().ReverseMap();
        }
    }
}
