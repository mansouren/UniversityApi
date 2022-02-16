using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Common.Security;
using UniversityApi.Common.Utilities;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.ViewModels;

namespace UniversityApi.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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

        public async Task<User> AddUser(User user)
        {
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            await userRepository.AddUser(user);
            await userRepository.Save();
            return user;
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
            User user= await userRepository.GetUserByusernameAndPassword(stringExtensions.ToLowerAndTrim(username), HashPassword);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
           return await userRepository.GetUsers();
        }
    }
}
