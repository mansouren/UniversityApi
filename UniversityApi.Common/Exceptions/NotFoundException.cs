﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Common.Exceptions
{
   public class NotFoundException : AppException
    {
        public NotFoundException(string message): base(message,ApiResultStatusCode.NotFound)
        {

        }
    }
}
