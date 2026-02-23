using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class SubjectRequestDTO
    {
        public int GradeLevel { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
    }
    public class UpdateSubjectRequestDTO : SubjectRequestDTO
    {
        public string Status { get; set; }
    }
}
