using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Common.Utilities
{
   public static class stringExtensions
    {
        public static string ToLowerAndTrim(this string input)
        {
           return input.Trim().ToLower();
        }
    }
}
