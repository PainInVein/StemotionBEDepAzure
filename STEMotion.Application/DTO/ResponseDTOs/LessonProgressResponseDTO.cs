using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class LessonProgressResponseDTO
    {
        public Guid LessonId { get; set; }
        public string LessonName { get; set; }
        public bool IsCompleted { get; set; }
        public int CompletionPercentage { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public int? EstimatedTime { get; set; }
    }
}
