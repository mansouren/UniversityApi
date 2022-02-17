using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;

namespace UniversityApi.Entities.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsers();
        Task<bool> IsExistEmail(string email);
        Task<bool> IsExistUsername(string username);
        Task<bool> IsExistUser(string username, string password);
        Task<User> GetUserByusernameAndPassword(string username, string password);
        
    }
}
