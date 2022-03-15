using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Dtos.Common;

namespace UniversityApi.Services.Dtos
{
    public class UserDto :  BaseDto<UserDto,User>
    {
        [Display(Name = "نقش کاربر")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        public int RoleId { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        [Compare(nameof(Password), ErrorMessage = "تکرار کلمه عبور صحیح نمی باشد")]
        public string RePassword { get; set; }
    }

    public class UserProfileDto : BaseDto<UserProfileDto,User>
    {

        [Display(Name = "نام")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        public string LastName { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(600, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        public string Address { get; set; }

        [Display(Name = "موبایل")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر داشته باشد")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نمی باشد")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        public string Email { get; set; }
    }

    public class UserResultDto : BaseDto<UserResultDto,User>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RoleTitle { get; set; }
        public string FullName { get; set; }

        public override void CustomMapping(IMappingExpression<User, UserResultDto> mappingExpression)
        {
            mappingExpression.ForMember(
                dest => dest.FullName,
                dest => dest.MapFrom(sourceMember => $"{FirstName} {LastName}"));
        }
    }
}
