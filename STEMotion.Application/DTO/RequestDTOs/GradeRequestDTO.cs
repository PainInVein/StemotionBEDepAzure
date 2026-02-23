using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class GradeRequestDTO
    {
        public int GradeLevel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class UpdateGradeRequest : GradeRequestDTO
    {
        public string Status { get; set; }
    }
}
