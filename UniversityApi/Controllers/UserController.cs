using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.ViewModels;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<User>> Create(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(registerViewModel);
            UserExistence userExistence = await userService.IsExistUsernameAndEmail(registerViewModel.Email, registerViewModel.UserName);

            switch (userExistence)
            {
                case UserExistence.UsernameAndEmailDuplicate:
                    return BadRequest("نام کاربری و ایمیل تکرای است!");
                    
                case UserExistence.EmailDuplicate:
                    return BadRequest("ایمیل تکرای است");
                case UserExistence.UsernameDuplicate:
                    return BadRequest("نام کاربری تکرای است");
               
            }

            User user = new User
            {
                Username = registerViewModel.UserName,
                Email = registerViewModel.Email,
                Password = registerViewModel.Password,

            };
           return await userService.AddUser(user);
        }
    }
}
