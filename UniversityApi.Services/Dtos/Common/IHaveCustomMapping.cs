using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Services.Dtos.Common
{
   public interface IHaveCustomMapping
    {
        void CreateMapping(Profile profile);
    }
}
