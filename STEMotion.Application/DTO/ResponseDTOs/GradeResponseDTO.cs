using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class GradeResponseDTO
    {
        public Guid GradeId { get; set; }
        public int GradeLevel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }


        public string Status { get; set; }
    }
}
