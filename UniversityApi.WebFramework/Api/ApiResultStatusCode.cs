﻿using System.ComponentModel.DataAnnotations;

namespace UniversityApi.WebFramework.Api
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 0,

        [Display(Name = "پارامترهای ارسالی معتبر نمی باشند")]
        BadRequest = 1,

        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError = 2,

        [Display(Name = "یافت نشد")]
        NotFound = 3,

        [Display(Name = "لیست خالی است")]
        ListEmpty = 4
    }
}
