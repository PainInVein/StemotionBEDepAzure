using System.Collections.Generic;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class PerformanceInsightResponseDTO
    {
        public List<string> Strengths { get; set; } = new List<string>();
        public List<string> Weaknesses { get; set; } = new List<string>();
        public List<string> SuggestedFocus { get; set; } = new List<string>();
        public Dictionary<string, double> SubjectPerformance { get; set; } = new Dictionary<string, double>(); // Subject name -> avg score
        public int TotalGamesPlayed { get; set; }
        public double AverageGameScore { get; set; }
        public int LearningStreak { get; set; } // consecutive days
        public string PerformanceTrend { get; set; } // "Improving", "Stable", "Declining"
    }
}
