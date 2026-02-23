using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.ResponseDTOs
{
    public class GameResponseDTO
    {
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string GameCode { get; set; }
        public string? Description { get; set; }
        public Guid LessonId { get; set; }
        public string? LessonName { get; set; }
        public string ConfigData { get; set; }
        public bool Status { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
    // Khi hs bat dau choi
    public class PlayGameDTO
    {
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string ConfigData { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }

    public class QuestionDTO
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }

}
