using STEMotion.Application.DTO.RequestDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IOtpService
    {
        Task SendOtpAsync(string email, string? userName = null, CreateUserRequestDTO? registrationData = null);
        Task<(bool IsValid, string? RegistrationData)> VerifyOtpAsync(string email, string otp);
    }
}
