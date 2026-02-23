using Microsoft.Extensions.Caching.Distributed;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _cache;
        #region cto
        public OtpService(IEmailService emailService, IDistributedCache cache)
        {
            _emailService = emailService;
            _cache = cache;
        }
        #endregion cto
        #region sendOTP
        public async Task SendOtpAsync(string email, string? userName = null, CreateUserRequestDTO? registrationData = null)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync($"OTP:{email}", otpCode, options);

            if (registrationData != null)
            {
                var json = JsonSerializer.Serialize(registrationData);
                await _cache.SetStringAsync($"REGISTRATION:{email}", json, options);
            }

            var displayName = userName;
            if (registrationData != null)
            {
                displayName = $"{registrationData.FirstName} {registrationData.LastName}".Trim();
            }
            displayName ??= "Quý khách";

            var subject = registrationData != null
                ? "Mã xác thực đăng ký STEMotion"
                : "Mã đổi mật khẩu STEMotion";


            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "OtpTemplate.html");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Không tìm thấy file OtpTemplate.html!");

            var htmlBody = await File.ReadAllTextAsync(templatePath);
            htmlBody = htmlBody.Replace("{{UserName}}", displayName)
                               .Replace("{{OTP_CODE}}", otpCode)
                               .Replace("{{ExpireMinutes}}", "5");

            await _emailService.SendEmailAsync(new MailRequests
            {
                ToAddress = email,
                Subject = $"[{otpCode}] {subject}",
                Body = htmlBody
            });
        }
        #endregion sendOTP
        #region VerifyOtpAsync
        public async Task<(bool IsValid, string? RegistrationData)> VerifyOtpAsync(string email, string otp)
        {
            var savedOtp = await _cache.GetStringAsync($"OTP:{email}");

            if (savedOtp != otp)
                return (false, null);
            var registrationJson = await _cache.GetStringAsync($"REGISTRATION:{email}");

            await _cache.RemoveAsync($"OTP:{email}");

            if (!string.IsNullOrEmpty(registrationJson))
            {
                await _cache.RemoveAsync($"REGISTRATION:{email}");
            }

            return (true, registrationJson);
        }
        #endregion VerifyOtpAsync
    }
}
