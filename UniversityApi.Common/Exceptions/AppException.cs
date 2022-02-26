using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Common.Exceptions
{
   public class AppException : Exception
    {
        public ApiResultStatusCode StatusCode { get; set; }
        public AppException(string message,ApiResultStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        
    }
}