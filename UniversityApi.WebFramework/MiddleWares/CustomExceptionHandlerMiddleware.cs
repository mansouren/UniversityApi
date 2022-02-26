using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.Common.Exceptions;
using UniversityApi.WebFramework.Api;

namespace UniversityApi.WebFramework.MiddleWares
{
   public class CustomExceptionHandlerMiddleware 
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (AppException ex)
            {
                logger.LogError(ex,ex.Message);
                var apiResult = new ApiResult(false, ex.StatusCode, ex.Message);
                string json = JsonConvert.SerializeObject(apiResult);
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(json);
            }

            catch (Exception ex)
            {
                logger.LogError(ex,"خطایی رخ داده است");
                var apiResult = new ApiResult(false, ApiResultStatusCode.ServerError);
                string json = JsonConvert.SerializeObject(apiResult);
                await httpContext.Response.WriteAsync(json);
                
            }
        }
    }
}
