using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos;
using UniversityApi.Services.ViewModels;

namespace UniversityApi.Services.Interfaces
{
   public interface IUserService 
    {
        Task UpdateLastLoginDate(User user, CancellationToken cancellationToken);
        //Task UpdateSecurityStamp(User user, CancellationToken cancellationToken);
        Task AddUser(User user, CancellationToken cancellationToken);
        Task UpdateProfile(User user, CancellationToken cancellationToken);
        Task<UserExistence> IsExistUsernameAndEmail(string email, string username);
        Task<bool> IsExistUser(string username, string password);
        Task<User> GetUserByUsernameAndPassword(string username, string password);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id, CancellationToken cancellationToken);
        
    }
}
