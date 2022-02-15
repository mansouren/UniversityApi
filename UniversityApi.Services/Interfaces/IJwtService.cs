using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;

namespace UniversityApi.Services.Interfaces
{
   public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
