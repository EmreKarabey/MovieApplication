using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.UpdateEmail;
using AutoMapper;
using CoreSecurity.Entities;

namespace Application.Features.Login.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateEmailResponse, User>().ReverseMap();
        }
    }
}
