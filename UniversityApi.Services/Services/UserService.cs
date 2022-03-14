using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.Common.Exceptions;
using UniversityApi.Common.Security;
using UniversityApi.Common.Utilities;
using UniversityApi.Data.Repositories;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.ViewModels;

namespace UniversityApi.Services.Services
{
    public class UserService : IUserService , IScopedDependency
    {
        private readonly IUserRepository userRepository;
        private readonly IRepository<Teacher> teacherRepo;
        private readonly IRepository<Student> studentRepo;

        //public IQueryable<User> UserQuery { get => userRepository.Table; }

        public UserService(IUserRepository userRepository, IRepository<Teacher> teacherRepo, IRepository<Student> studentRepo)
        {
            this.userRepository = userRepository;
            this.teacherRepo = teacherRepo;
            this.studentRepo = studentRepo;
        }

        public async Task<UserExistence> IsExistUsernameAndEmail(string email, string username)
        {
            bool usernameValidation = await userRepository.IsExistUsername(username.Trim().ToLower());
            bool emailValidation = await userRepository.IsExistEmail(email.Trim().ToLower());

            if (usernameValidation && emailValidation)
                return UserExistence.UsernameAndEmailDuplicate;
            else if (emailValidation)
                return UserExistence.EmailDuplicate;
            else if (usernameValidation)
                return UserExistence.UsernameDuplicate;
            else
                return UserExistence.Ok;
        }

        //public async Task<User> AddUser(User user, CancellationToken cancellationToken)
        //{
        //    UserExistence userExistence = await IsExistUsernameAndEmail(user.Email, user.Username);

        //    switch (userExistence)
        //    {
        //        case UserExistence.UsernameAndEmailDuplicate:
        //            throw new BadRequestException("نام کاربری و ایمیل تکرای است!");

        //        case UserExistence.EmailDuplicate:
        //            throw new BadRequestException("ایمیل تکرای است");
        //        case UserExistence.UsernameDuplicate:
        //            throw new BadRequestException("نام کاربری تکرای است");

        //    }
        //    user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
        //    await userRepository.AddAsync(user, cancellationToken);
        //    return user;
        //}

        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            user.Username = stringExtensions.GenerateCode();
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            await userRepository.AddAsync(user, cancellationToken);

            if (user.RoleId == 3)
            {
                Teacher teacher = new Teacher
                {
                    UserId = user.Id,
                    TeacherCode = user.Username
                };
                await teacherRepo.AddAsync(teacher, cancellationToken);
            }

            else if (user.RoleId == 4)
            {
                Student student = new Student
                {
                    StudentCode = user.Username,
                    UserId = user.Id
                };
                await studentRepo.AddAsync(student, cancellationToken);
            }
           
        }

        public async Task<bool> IsExistUser(string username, string password)
        {
            string HashPassword = PasswordHelper.EncodePasswordMd5(password);
            bool exist = await userRepository.IsExistUser(username.Trim().ToLower(), HashPassword);
            return exist;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            string HashPassword = PasswordHelper.EncodePasswordMd5(password);
            User user = await userRepository.GetUserByusernameAndPassword(stringExtensions.ToLowerAndTrim(username), HashPassword);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await userRepository.GetUsers();
        }

        //public async Task UpdateSecurityStamp(User user, CancellationToken cancellationToken)
        //{
        //    user.SecurityStamp = Guid.NewGuid();
        //    await userRepository.UpdateAsync(user, cancellationToken);
        //}

        public async Task<User> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user =await userRepository.TableAsNoTracking.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task UpdateLastLoginDate(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            await userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task UpdateProfile(User user, CancellationToken cancellationToken)
        {
            user.FirstName = user.FirstName.Trim().ToLower();
            user.LastName = user.LastName.Trim().ToLower();
            user.Email = user.Email.Trim().ToLower();
            await userRepository.UpdateAsync(user, cancellationToken);
        }

        
    }
}
