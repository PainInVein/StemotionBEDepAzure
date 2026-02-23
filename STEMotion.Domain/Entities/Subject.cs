using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Subject
    {
        public Guid SubjectId { get; set; }
        public Guid GradeId { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OrderIndex { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}
