using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class CreateUserRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String? RoleName { get; set; } 
        public int? GradeLevel { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public class UpdateUserRequestDTO
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public Guid? RoleId { get; set; }
        public int? GradeLevel { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
    }
    public class ChangePasswordRequestDTO
    {
        [Required]
        public string Email { get; set; } 

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OtpCode { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class GoogleRequestDTO
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string? RoleName { get; set; }
    }
}
