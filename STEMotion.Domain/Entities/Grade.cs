using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    // Khối lớp: Lớp 1, Lớp 2, Lớp 3, ...
    public class Grade
    {
        public Guid GradeId { get; set; }
        public int GradeLevel { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int OrderIndex { get; set; }
        public string Description { get; set; }

    }
}
