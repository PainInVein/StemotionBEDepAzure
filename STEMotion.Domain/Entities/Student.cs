using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AvatarUrl { get; set; }
        public int? GradeLevel { get; set; }             
        public DateTime CreatedAt { get; set; }
        public virtual User Parent { get; set; }
        public virtual ICollection<StudentProgress> StudentProgress { get; set; }
        public virtual ICollection<GameResult> GameResults { get; set; }
        public string? Status { get; set; }
    }
}
