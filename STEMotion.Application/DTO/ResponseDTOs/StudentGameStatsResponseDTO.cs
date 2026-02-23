using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class StudentGameStatsResponseDTO
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; }
        public int AttemptCount { get; set; }
        public double? BestScore { get; set; }
        public int? BestCorrectAnswers { get; set; }
        public DateTime? LastPlayedAt { get; set; }
    }
}
