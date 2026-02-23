using System;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class StudentLeaderboardDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public double TotalScore { get; set; }
        public int Rank { get; set; }
    }
}
