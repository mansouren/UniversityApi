using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;

namespace UniversityApi.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }


        public async Task<User> GetUserByusernameAndPassword(string username, string password)
        {
            var user = await Table.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            //dbContext.Users output is DbSet<User>
            return await TableAsNoTracking.ToListAsync();//Equals dbContext.Users.ToListAsync()
        }

        public async Task<bool> IsExistEmail(string email)
        {
            return await TableAsNoTracking.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistUser(string username, string password)
        {
            return await TableAsNoTracking.AnyAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> IsExistUsername(string username)
        {
            return await TableAsNoTracking.AnyAsync(u => u.Username == username);
        }


    }
}
