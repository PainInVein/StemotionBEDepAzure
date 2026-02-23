using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class CreateGameDTO
    {
        public string Name { get; set; }
        public string GameCode { get; set; }
        public string? Description { get; set; }
        public Guid LessonId { get; set; }
        public string ConfigData { get; set; }
        public string? ThumbnailUrl { get; set; }
    }

    public class UpdateGameDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ConfigData { get; set; }
        public bool? Status { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
