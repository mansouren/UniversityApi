using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Services.ViewModels
{
    public class RegisterViewModel : IValidatableObject
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        public string UserName { get; set; }

        public int RoleId { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
         public string Password { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر داشته باشد")]
        [Required(ErrorMessage = "لطفا مقداری وارد کنید")]
        [Compare(nameof(Password),ErrorMessage ="تکرار کلمه عبور صحیح نمی باشد")]
        public string RePassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("نام کاربری نمی تواند test باشد", new string[] { nameof(UserName) });
            if (Password.Equals("123456"))
                yield return new ValidationResult("پسورد نمی تواند 123456 باشد", new string[] { nameof(Password) });
        }
    }

    public enum UserExistence
    {
        UsernameAndEmailDuplicate,
        EmailDuplicate,
        UsernameDuplicate,
        Ok
    }
}
