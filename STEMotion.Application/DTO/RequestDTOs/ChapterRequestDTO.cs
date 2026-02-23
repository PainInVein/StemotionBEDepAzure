using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class ChapterRequestDTO
    {
        public string SubjectName { get; set; }
        public int GradeLevel { get; set; }
        public string ChapterName { get; set; }
    }
    public class UpdateChapterRequestDTO : ChapterRequestDTO
    {
        public string Status { get; set; }
    }
}
