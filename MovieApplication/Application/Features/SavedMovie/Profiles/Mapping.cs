using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SavedMovie.Command.Create;
using Application.Features.SavedMovie.Command.Delete;
using Application.Features.SavedMovie.Command.Update;
using Application.Features.SavedMovie.Queries.GetById;
using Application.Features.SavedMovie.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.SavedMovie.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.SavedMovie, CreatedSavedMovieCommand>().ReverseMap();
            CreateMap<Domain.Entities.SavedMovie, CreatedSavedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.SavedMovie, DeletedSavedMovieResponse>().ReverseMap();

            CreateMap<Domain.Entities.SavedMovie, UpdateSavedMovieResponse>().ReverseMap();
            CreateMap<Domain.Entities.SavedMovie, UpdateSavedMovieCommand>().ReverseMap();

            CreateMap<Domain.Entities.SavedMovie, GetByIdSavedMovieDto>().ReverseMap();


            CreateMap<Domain.Entities.SavedMovie, GetListSavedMovieDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.SavedMovie>, GetListResponse<GetListSavedMovieDto>>().ReverseMap();
        }
    }
}
