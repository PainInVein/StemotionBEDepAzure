using STEMotion.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string entityName, object key)
        : base($"{entityName} với định danh {key} không tồn tại.", "NOT_FOUND", 404) { }

        public NotFoundException(string message)
            : base(message, "NOT_FOUND", 404) { }
    }

    public class AlreadyExistsException : BaseException
    {
        public AlreadyExistsException(string entityName, string value)
            : base($"{entityName} với giá trị {value} đã tồn tại trong hệ thống.", "ALREADY_EXISTS", 409) { }
    }

    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
            : base(message, "BAD_REQUEST", 400) { }
    }

    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message = "Bạn không có quyền thực hiện hành động này.")
            : base(message, "FORBIDDEN", 403) { }
    }

    public class ValidationException : BaseException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("Dữ liệu đầu vào không hợp lệ.", "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }
    }
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "Bạn chưa đăng nhập hoặc phiên đăng nhập đã hết hạn.")
            : base(message, "UNAUTHORIZED", 401) { }
    }
    public class InternalServerException : BaseException
    {
        public InternalServerException(string message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.")
            : base(message, "INTERNAL_SERVER_ERROR", 500) { }
    }
    public class ServiceUnavailableException : BaseException
    {
        public ServiceUnavailableException(string serviceName)
            : base($"Dịch vụ {serviceName} hiện không khả dụng.", "SERVICE_UNAVAILABLE", 503) { }
    }

}
