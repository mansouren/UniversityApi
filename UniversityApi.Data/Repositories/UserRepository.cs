using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;

namespace UniversityApi.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository, IScopedDependency
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

        //public async Task<IEnumerable<User>> GetUsers()
        //{
        //    //dbContext.Users output is DbSet<User>
        //    return await TableAsNoTracking.ToListAsync();//Equals dbContext.Users.ToListAsync()
        //}

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

        public override Task UpdateAsync(User entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            entity.SecurityStamp = Guid.NewGuid();
            return base.UpdateAsync(entity, cancellationToken, saveNow);
        }

        public override void Update(User entity, bool saveNow = true)
        {
            entity.SecurityStamp = Guid.NewGuid();
            base.Update(entity, saveNow);
        }

        public override Task UpdateRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            foreach (var user in entities)
            {
                user.SecurityStamp = Guid.NewGuid();
            }
            return base.UpdateRangeAsync(entities, cancellationToken, saveNow);
        }

        public override void UpdateRange(IEnumerable<User> entities, bool saveNow = true)
        {
            foreach (var user in entities)
            {
                user.SecurityStamp = Guid.NewGuid();
            }
            base.UpdateRange(entities, saveNow);
        }

    }
}
