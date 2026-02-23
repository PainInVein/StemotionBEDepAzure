using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ChapterProgressResponseDTO
    {
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }
        public double CompletionPercentage { get; set; } // % hoàn thành chương
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public List<LessonProgressResponseDTO> LessonProgress { get; set; }
    }
}
