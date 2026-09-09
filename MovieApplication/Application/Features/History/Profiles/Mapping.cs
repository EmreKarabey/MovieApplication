using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.History.Command.Create;
using Application.Features.History.Command.Delete;
using Application.Features.History.Command.Update;
using Application.Features.History.Queries.GetById;
using Application.Features.History.Queries.GetList;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using AutoMapper;
using CoreApplication.Responses;
using CorePersistence.Paginate;

namespace Application.Features.History.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.History, CreatedHistoryCommand>().ReverseMap();
            CreateMap<Domain.Entities.History, CreatedHistoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.History, DeletedHistoryResponse>().ReverseMap();

            CreateMap<Domain.Entities.History, UpdateHistoryResponse>().ReverseMap();
            CreateMap<Domain.Entities.History, UpdateHistoryCommand>().ReverseMap();

            CreateMap<Domain.Entities.History, GetByIdHistoryDto>().ReverseMap();


            CreateMap<Domain.Entities.History, GetListHistoryDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.History>, GetListResponse<GetListHistoryDto>>().ReverseMap();
        }
    }
}
