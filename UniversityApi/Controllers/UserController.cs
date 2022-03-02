using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.ViewModels;
using UniversityApi.WebFramework.Api;
using UniversityApi.WebFramework.Filters;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;
        private readonly IJwtService jwtService;

        public UserController(IUserService userService,ILogger<UserController> logger, IJwtService jwtService)
        {
            this.userService = userService;
            this.logger = logger;
            this.jwtService = jwtService;
        }

        [HttpGet]
        //[Authorize(Roles="User")]
        //[ApiResultFilter]
        public async Task<IEnumerable<User>> Get()
        {
            return await userService.GetUsers();
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
           var user= await userService.GetUserById(id, cancellationToken);
            if (user == null)
                return NotFound();

            return user;
        }

        //[HttpPost("[action]")]
        ////[Authorize]
        //public async Task<ApiResult<User>> Create(RegisterViewModel registerViewModel, CancellationToken cancellationToken)
        //{
        //    //if (!ModelState.IsValid) (We had Automated ModelStateValidation in ApiResultFilterAttribute - BadRequstObjectResult)
        //    //    return BadRequest(registerViewModel);

        //    User user = new User
        //    {
        //        Username = registerViewModel.UserName,
        //        Email = registerViewModel.Email,
        //        Password = registerViewModel.Password,
        //        RoleId = registerViewModel.RoleId

        //    };
        //    return Ok(await userService.AddUser(user,cancellationToken));
        //}
        
        
        [HttpPost("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task Create(UserDto userDto, CancellationToken cancellationToken)
        {
            User user = new User
            {
               RoleId = userDto.RoleId,
               Password = userDto.Password,
               IsActive = false
            };
            await userService.AddUser(user, cancellationToken);
            
        }

        [HttpGet("[action]")]
        public async Task<string> Token(string username, string password)
        {
            bool exist = await userService.IsExistUser(username, password);
            if (!exist)
                throw new BadHttpRequestException("کاربری با این مشخصات یافت نشد!");

            User user = await userService.GetUserByUsernameAndPassword(username, password);

            return jwtService.GenerateToken(user);

        }
    }
}
