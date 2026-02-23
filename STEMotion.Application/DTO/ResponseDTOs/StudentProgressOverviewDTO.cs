using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class StudentProgressOverviewDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int GradeLevel { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalChapters { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int OverallCompletionPercentage { get; set; }
        public DateTime? LastActivityDate { get; set; }
        
        public int LearningStreak { get; set; }

        public double TotalPoints { get; set; } // Changed to double to match GameResult.Score
        public int CurrentLevel { get; set; }
        public List<SubjectProgressResponseDTO> Subjects { get; set; }
    }
}
