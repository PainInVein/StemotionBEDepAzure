using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class SubmitGameResultRequestDto
    {
        public Guid StudentId { get; set; }
        public Guid GameId { get; set; }
        public double Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int PlayDurations { get; set; }
    }

    public class HistoryGameResultDto
    {
        public Guid GameResultId { get; set; }
        public Guid GameId { get; set; }
        public string GameName { get; set; }
        public double Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int PlayDuration { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
