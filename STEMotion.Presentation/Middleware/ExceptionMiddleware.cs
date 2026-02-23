using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;

namespace STEMotion.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                await HandleExceptionAsync(context, ex.StatusCode, ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                // await HandleExceptionAsync(context, 500, ex.Message, ex.StackTrace);
                // Lấy tất cả inner exceptions để debug
                var errorMessage = ex.Message;
                var innerEx = ex.InnerException;
                var level = 1;
                
                while (innerEx != null && level <= 5)
                {
                    errorMessage += $" | Inner{level}: {innerEx.Message}";
                    innerEx = innerEx.InnerException;
                    level++;
                }
                
                await HandleExceptionAsync(context, 500, errorMessage, ex.StackTrace);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message, string errorCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = ResponseDTO<object>.Fail(message, errorCode);

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
