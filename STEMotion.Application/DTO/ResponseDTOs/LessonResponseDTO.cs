using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class LessonResponseDTO
    {
        public Guid LessonId { get; set; }
        public string ChapterName { get; set; }
        public int GradeLevel { get; set; }
        public string LessonName { get; set; }
        public int? EstimatedTime { get; set; }
        public int OrderIndex { get; set; }

        public string Status { get; set; }  
    }
}
