using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class OtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class OtpVerificationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be 6 digits")]
        public string OtpCode { get; set; }
    }
}
