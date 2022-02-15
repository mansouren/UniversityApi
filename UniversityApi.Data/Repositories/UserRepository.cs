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
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext context;

        public UserRepository(DatabaseContext context)
        {
            this.context = context;
        }
        public async Task AddUser(User user)
        {
           await context.Users.AddAsync(user);
           
        }

        public async Task<bool> IsExistEmail(string email)
        {
           return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistUsername(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task Save()
        {
           await context.SaveChangesAsync();
        }
    }
}
