using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class CreateGameRequestDto
    {
        [Required(ErrorMessage = "Tên game là bắt buộc")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mã game là bắt buộc")]
        [MaxLength(50)]
        public string GameCode { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url(ErrorMessage = "URL thumbnail không hợp lệ")]
        public string? ThumbnailUrl { get; set; }
    }
}
