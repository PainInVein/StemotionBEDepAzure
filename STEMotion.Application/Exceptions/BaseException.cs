using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Exceptions
{
    public class BaseException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get;}

        public BaseException(string message, string errorCode, int statusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
