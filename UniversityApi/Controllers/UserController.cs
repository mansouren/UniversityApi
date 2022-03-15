using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper mapper;

        public UserController(IUserService userService,
            ILogger<UserController> logger,
            IJwtService jwtService,
            IMapper mapper)
        {
            this.userService = userService;
            this.logger = logger;
            this.jwtService = jwtService;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles="User")]
        //[ApiResultFilter]
        public async Task<IEnumerable<UserResultDto>> Get()
        {
            var userQuery =  userService.GetUsers();
            var dtolst = await userQuery.ProjectTo<UserResultDto>(mapper.ConfigurationProvider).ToListAsync();
            return dtolst;
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<UserResultDto>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(id, cancellationToken);
            if (user == null)
                return NotFound();
            var dto = UserResultDto.FromEntity(mapper, user);
            return dto;
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
        [Authorize(Roles = "Admin")]
        public async Task Create(UserDto userDto, CancellationToken cancellationToken)
        {
            #region Old Code
            //User user = new User
            //{
            //    RoleId = userDto.RoleId,
            //    Password = userDto.Password,
            //    IsActive = false
            //};
            #endregion

            var user = userDto.ToEntity(mapper);

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

        [HttpPut("[Action]")]
        public async Task<UserResultDto> CompeleteProfile(int id, UserProfileDto profileDto, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(id, cancellationToken);
            if(user == null)
            {
                throw new BadHttpRequestException("کاربری با این مشخصات یافت نشد!");
            }
            #region OldCode
            //user.FirstName = profileDto.FirstName;
            //user.LastName = profileDto.LastName;
            //user.Phone = profileDto.Phone;
            //user.Address = profileDto.Address;
            //user.Email = profileDto.Email;
            #endregion
            //mapper.Map(profileDto, user); OldCode
            profileDto.ToEntity(mapper, user); //Using BaseDto's Methods

            await userService.UpdateProfile(user, cancellationToken);

            #region OldCode
            //var dto = new UserResultDto
            //{
            //    Username = user.Username,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Phone = user.Phone,
            //    Address = user.Address,
            //    RoleTitle = user.Role.Title
            //};
            #endregion

            //var dto = mapper.Map<UserResultDto>(user); OldCode
            var dto = UserResultDto.FromEntity(mapper,user);//Using BaseDto's Methods
            return dto;
        }
    }
}
