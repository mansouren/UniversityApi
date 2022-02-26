using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IWebHostEnvironment env;

        public CustomExceptionHandlerMiddleware(RequestDelegate next
            , ILogger<CustomExceptionHandlerMiddleware> logger
            , IWebHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

            try
            {
                await next(httpContext);
            }
            catch (AppException ex)
            {
                logger.LogError(ex, ex.Message);
                httpStatusCode = ex.HttpStatusCode;
                apiStatusCode = ex.ApiStatusCode;

                if (env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };

                    if (ex.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", ex.InnerException.Message);
                        dic.Add("InnerException.StackTrace", ex.InnerException.StackTrace);
                    }
                    if (ex.AdditionalData != null)
                    {
                        dic.Add("AdditionalData", JsonConvert.SerializeObject(ex.AdditionalData));
                    }

                    message = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    message = ex.Message;
                }

                await WriteToResponseAsync();

            }

            catch(SecurityTokenExpiredException ex)
            {
                logger.LogError(ex, ex.Message);
                SetUnAuthorizeResponse(ex);
                await WriteToResponseAsync();
            }
            catch(UnauthorizedAccessException ex)
            {
                logger.LogError(ex, ex.Message);
                SetUnAuthorizeResponse(ex);
                await WriteToResponseAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                if (env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    message = JsonConvert.SerializeObject(dic);
                }
                
                   await WriteToResponseAsync();
                
            }

            async Task WriteToResponseAsync()
            {
                if (httpContext.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var apiResult = new ApiResult(false, apiStatusCode, message);
                string json = JsonConvert.SerializeObject(apiResult);
                httpContext.Response.StatusCode =(int)httpStatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception exception)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                apiStatusCode = ApiResultStatusCode.UnAuthorized;

                if (env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace
                    };
                    if (exception is SecurityTokenExpiredException tokenException)
                        dic.Add("Expires", tokenException.Expires.ToString());

                    message = JsonConvert.SerializeObject(dic);
                }
               
            }
        }
    }
}
