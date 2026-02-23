using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IJWTService _jwtService;
        public AuthController(IOtpService otpService, IUnitOfWork unitOfWork, IUserService userService, IConfiguration configuration, IJWTService jwtService)
        {
            _otpService = otpService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        [EndpointDescription("API này đăng nhập User")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _userService.LoginUser(loginRequestDTO);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("accessToken", result, cookieOptions);
            var principal = _jwtService.ValidateToken(result);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            var userResponse = new UserResponseDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                GradeLevel = user.GradeLevel,
                AvatarUrl = user.AvatarUrl,
                Status = user.Status,
                RoleName = user.Role?.Name
            };


            return Ok(ResponseDTO<UserResponseDTO>.Success(userResponse, "Đăng nhập thành công"));
        }

        [HttpGet("me")]
        [EndpointDescription("Lấy thông tin user từ cookie")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var token = Request.Cookies["accessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(ResponseDTO<object>.Fail("Không tìm thấy token", "UNAUTHORIZED"));
                }

                var principal = _jwtService.ValidateToken(token);
                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return Unauthorized(ResponseDTO<object>.Fail("User không tồn tại", "USER_NOT_FOUND"));
                }

                var userResponse = new UserResponseDTO
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    GradeLevel = user.GradeLevel,
                    AvatarUrl = user.AvatarUrl,
                    Status = user.Status,
                    RoleName = user.Role?.Name
                };

                return Ok(ResponseDTO<UserResponseDTO>.Success(userResponse, "Lấy thông tin user thành công"));
            }
            catch (Exception)
            {
                return Unauthorized(ResponseDTO<object>.Fail("Token không hợp lệ hoặc đã hết hạn", "INVALID_TOKEN"));
            }
        }

        [HttpGet("google-login")]
        [EndpointDescription("API này để login Google")]
        public IActionResult GoogleLogin([FromQuery] string role = "student")
        {
            var redirectUri = _configuration["Authentication:Google:RedirectUri"];
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };
            properties.Items.Add("RoleName", role);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                throw new UnauthorizedException("Google authentication failed");

            result.Properties.Items.TryGetValue("RoleName", out var roleName);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var googleRequestDTO = new GoogleRequestDTO
            {
                Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                FirstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                AvatarUrl = claims?.FirstOrDefault(c => c.Type == "picture")?.Value,
                RoleName = roleName
            };

            var response = await _userService.AuthenticateGoogleUserAsync(googleRequestDTO);
            //var token = response.Result;
            //return Redirect($"http://localhost:5173/login-success?token={token}");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("accessToken", response, cookieOptions);
            return Redirect("http://localhost:5173/google-callback");
        }

        [HttpPost("register/send-otp")]
        [EndpointDescription("Bước 1: Gửi mã OTP để xác thực email khi đăng ký (FE phải gửi roleName: student hoặc parent)")]
        public async Task<IActionResult> SendRegistrationOtp([FromBody] CreateUserRequestDTO request)
        {
            await _userService.RequestRegistrationOtpAsync(request);
            return Ok(ResponseDTO<object>.Success(null, "Mã OTP đã được gửi"));
        }

        [HttpPost("register/verify-otp")]
        [EndpointDescription("Bước 2: Xác thực OTP và tạo tài khoản")]
        public async Task<IActionResult> VerifyOtpAndRegister([FromBody] OtpVerificationRequest request)
        {
            var result = await _userService.VerifyOtpAndRegisterAsync(request.Email, request.OtpCode);
            return Ok(ResponseDTO<UserResponseDTO>.Success(result, "Đăng ký thành công"));
        }

        [HttpPost("password/send-otp")]
        [EndpointDescription("Bước 1: Gửi mã OTP để đổi mật khẩu")]
        public async Task<IActionResult> SendPasswordResetOtp([FromBody] OtpRequest request)
        {
            await _userService.RequestPasswordResetOtpAsync(request.Email);
            return Ok(ResponseDTO<object>.Success(null, "Mã OTP đặt lại mật khẩu đã được gửi"));
        }
        [HttpPost("password/reset")]
        [EndpointDescription("Bước 2: Xác thực OTP và đổi mật khẩu")]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequestDTO request)
        {
            var result = await _userService.ChangePassword(request);
            return Ok(ResponseDTO<UserResponseDTO>.Success(result, "Đổi mật khẩu thành công"));
        }

        [HttpPost("logout")]
        [EndpointDescription("Đăng xuất - xóa cookie")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return Ok(ResponseDTO<object>.Success(null, "Đăng xuất thành công"));
        }
    }
}
