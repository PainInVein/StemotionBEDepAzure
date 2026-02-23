using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs.StudentReqDTOs
{
    public class AddChildrenRequestDTO
    {
        public Guid ParentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? GradeLevel { get; set; }
    }
}
