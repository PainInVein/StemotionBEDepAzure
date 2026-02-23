using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class SubjectProgressResponseDTO
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public double CompletionPercentage { get; set; } // % hoàn thành môn học
        public int TotalChapters { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedChapters { get; set; }
        public int CompletedLessons { get; set; }
        public List<ChapterProgressResponseDTO> ChapterProgress { get; set; }
    }
}
