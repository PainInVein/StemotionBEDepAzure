using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Domain.Entities
{
    public class GameResult
    {
        public Guid GameResultId { get; set; }
        public Guid StudentId { get; set; }
        public Guid GameId { get; set; }
        public double Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int PlayDuration { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.Now;

        public virtual Student? Student { get; set; }
        public virtual Game? Game { get; set; }
    }
}
