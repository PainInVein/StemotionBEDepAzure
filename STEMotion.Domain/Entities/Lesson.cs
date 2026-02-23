using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Lesson
    {
        public Guid LessonId { get; set; }
        public Guid ChapterId { get; set; }
        public string LessonName { get; set; }
        public int? EstimatedTime { get; set; }
        public string Status { get; set; }
        public int OrderIndex { get; set; }

        public virtual Chapter Chapter { get; set; }
        public virtual ICollection<LessonContent> LessonContents { get; set; }
        public virtual ICollection<StudentProgress> StudentProgress { get; set; }

    }
}
