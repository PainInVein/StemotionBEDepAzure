using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ResponseDTO<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
        public string? ErrorCode { get; set; }
        public object? Errors { get; set; }

        public ResponseDTO() { }
        public ResponseDTO(string message, bool isSuccess, T? result)
        {
            Message = message;
            IsSuccess = isSuccess;
            Result = result;
        }
        public static ResponseDTO<T> Success(T result, string message = "Thành công")
        {
            return new ResponseDTO<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result
            };
        }

        public static ResponseDTO<T> Fail(string message, string errorCode, object? errors = null)
        {
            return new ResponseDTO<T>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Errors = errors
            };
        }
    }
}
