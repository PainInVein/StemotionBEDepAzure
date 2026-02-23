using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class SubjectResponseDTO
    {
        public Guid SubjectId { get; set; }
        public int GradeLevel { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }

        public string Status { get; set; }
    }
}
