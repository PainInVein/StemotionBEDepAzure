
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class LessonRequestDTO
    {
        public Guid ChapterId { get; set; }
        public string LessonName { get; set; } = null!;
        //public int? EstimatedTime { get; set; }
    }
    public class UpdateLessonRequestDTO : LessonRequestDTO
    {
        public string Status { get; set; }
    }
}
