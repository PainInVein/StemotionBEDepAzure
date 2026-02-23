using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class LessonContent
    {
        public Guid LessonContentId { get; set; }
        public Guid LessonId { get; set; }
        public string ContentType { get; set; }
        public string? TextContent { get; set; }   
        public string? MediaUrl { get; set; }
        public string? FormulaLatex { get; set; }
        public int OrderIndex { get; set; }
        public string Status { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
