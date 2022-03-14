using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos;
using UniversityApi.WebFramework.AutoMApper;

namespace UniversityApi.CustomMappings
{
    public class UserCustomMapping : IHaveCustomMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<User, UserProfileDto>().ReverseMap().ForMember(u => u.Role, opt => opt.Ignore());

            profile.CreateMap<User, UserResultDto>().ReverseMap();
        }
    }
}
