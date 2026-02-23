using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class StudentProgress
    {
        public Guid StudentProgressId { get; set; }
        public Guid StudentId { get; set; }
        public Guid LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int? CompletionPercentage { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public string Status { get; set; }
        public virtual Student Student { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
