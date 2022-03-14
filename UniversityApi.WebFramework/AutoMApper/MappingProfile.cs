using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos;

namespace UniversityApi.WebFramework.AutoMApper
{
   public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>().ReverseMap()
                                      .ForMember(u => u.Role,opt => opt.Ignore());
            CreateMap<User, UserResultDto>().ReverseMap();
        }
    }
}
