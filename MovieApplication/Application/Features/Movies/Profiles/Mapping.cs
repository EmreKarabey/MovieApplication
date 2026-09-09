using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Movies.Commands.Create;
using Application.Features.Movies.Commands.Delete;
using Application.Features.Movies.Commands.Update;
using Application.Features.Movies.Queries.GetById;
using Application.Features.Movies.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;
using Domain.Entities;

namespace Application.Features.Movies.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreatedMovieResponse, Movie>().ReverseMap();
            CreateMap<CreatedMovieCommand, Movie>().ReverseMap();

            CreateMap<UpdatedMovieResponse, Movie>().ReverseMap();
            CreateMap<UpdatedMovieCommand, Movie>().ReverseMap();

            CreateMap<DeletedMovieResponse, Movie>().ReverseMap();

            CreateMap<GetMoviesListDto, Movie>().ReverseMap();
            CreateMap<GetListResponse<GetMoviesListDto>, Paginate<Movie>>().ReverseMap();

            CreateMap<GetByIdMovieDto, Movie>().ReverseMap();
        }
    }
}
