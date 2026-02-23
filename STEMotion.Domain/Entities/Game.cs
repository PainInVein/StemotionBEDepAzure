using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class Game
    {
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string GameCode { get; set; }
        public string? Description { get; set; }
        public Guid LessonId { get; set; }
        public string ConfigData { get; set; }
        public bool Status { get; set; } = true;
        public string? ThumbnailUrl { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<GameResult> GameResults { get; set; }
    }
}
