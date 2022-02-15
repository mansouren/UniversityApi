using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.ViewModels;

namespace UniversityApi.Services.Interfaces
{
   public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<UserExistence> IsExistUsernameAndEmail(string email, string username);
        Task<bool> IsExistUser(string username, string password);
        Task<User> GetUserByUsernameAndPassword(string username, string password);
    }
}
