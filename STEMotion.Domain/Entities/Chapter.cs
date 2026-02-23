using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Chapter
    {
        public Guid ChapterId { get; set; }
        public Guid SubjectId { get; set; }
        public string ChapterName { get; set; }
        public string Status { get; set; }
        public int OrderIndex { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
