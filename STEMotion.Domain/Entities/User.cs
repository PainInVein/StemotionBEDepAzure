using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid RoleId { get; set; }
        public int? GradeLevel { get; set; }
        public string AvatarUrl { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        public virtual ICollection<StudentProgress> StudentProgress { get; set; }
    }
}
