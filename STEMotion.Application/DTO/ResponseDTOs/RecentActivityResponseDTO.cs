using System;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class RecentActivityResponseDTO
    {
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; } // "Lesson" or "Game"
        public Guid ReferenceId { get; set; } // LessonId or GameId
        public DateTime ActivityDate { get; set; }
        public int DurationMinutes { get; set; }
        public double Score { get; set; }
        public string Status { get; set; } // "Completed", "In Progress"
        public int? CorrectAnswers { get; set; }
        public int? TotalQuestions { get; set; }
    }
}
