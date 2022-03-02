using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;

namespace UniversityApi.Services.Dtos
{
    public class UserDto
    {
        [Display(Name = "نقش کاربر")]
        [Required(ErrorMessage ="لطفا مقداری وارد کنید")]
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
}
