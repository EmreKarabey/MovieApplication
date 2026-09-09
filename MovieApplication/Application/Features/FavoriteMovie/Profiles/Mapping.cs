using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.FavoriteMovie.Command.Create;
using Application.Features.FavoriteMovie.Command.Delete;
using Application.Features.FavoriteMovie.Command.Update;
using Application.Features.FavoriteMovie.Queries.GetById;
using Application.Features.FavoriteMovie.Queries.GetList;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.FavoriteMovie.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.FavoriteMovie, CreatedFavoriteMovieCommand>().ReverseMap();
            CreateMap<Domain.Entities.FavoriteMovie, CreatedFavoriteMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.FavoriteMovie, DeletedFavoriteMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.FavoriteMovie, UpdateFavoriteMovieResponse>().ReverseMap();
            CreateMap<Domain.Entities.FavoriteMovie, UpdateFavoriteMovieCommand>().ReverseMap();

            CreateMap<Domain.Entities.FavoriteMovie, GetByIdFavoriteMovieDto>().ReverseMap();


            CreateMap<Domain.Entities.FavoriteMovie, GetListFavoriteMovieDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.FavoriteMovie>, GetListResponse<GetListFavoriteMovieDto>>().ReverseMap();
        }
    }
}
