using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.WebFramework.AutoMApper
{
    public class CustomMappingProfile : Profile
    {
        public CustomMappingProfile(IEnumerable<IHaveCustomMapping> haveCustomMappings)
        {
            foreach (var item in haveCustomMappings)
            {
                item.CreateMapping(this);
            }
        }
    }
}
