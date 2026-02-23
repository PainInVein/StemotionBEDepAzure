using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class ParentStudentListDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int GradeLevel { get; set; }
        public string AvatarUrl { get; set; }
        public int OverallCompletionPercentage { get; set; }
        public DateTime? LastActivityDate { get; set; }

    }
}
