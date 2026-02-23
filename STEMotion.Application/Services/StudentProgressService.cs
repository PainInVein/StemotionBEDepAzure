using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class StudentProgressService : IStudentProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentProgressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<LessonProgressResponseDTO> GetLessonProgressAsync(Guid studentId, Guid lessonId)
        {
            var progress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLessonAsync(studentId, lessonId);

            if (progress == null)
            {
                throw new AlreadyExistsException(studentId.ToString(), lessonId.ToString());
            }

            return _mapper.Map<LessonProgressResponseDTO>(progress);
        }

        public async Task<LessonProgressResponseDTO> StartLessonAsync(Guid studentId, Guid lessonId)
        {
            var existingProgress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLessonAsync(studentId, lessonId);

            if (existingProgress != null)
            {
                existingProgress.LastAccessedAt = DateTime.UtcNow;
                
                if (existingProgress.StartedAt == null)
                {
                    existingProgress.StartedAt = DateTime.UtcNow;
                    existingProgress.Status = "in_progress";
                }

                //_unitOfWork.StudentProgressRepository.Update(existingProgress);
                await _unitOfWork.SaveChangesAsync();
                
                return _mapper.Map<LessonProgressResponseDTO>(existingProgress);
            }

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new NotFoundException("Lesson", lessonId);

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student", studentId);

            var newProgress = new StudentProgress
            {
                StudentId = studentId,
                LessonId = lessonId,
                StartedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow,
                Status = "in_progress",
                CompletionPercentage = 0,
                IsCompleted = false
            };

            await _unitOfWork.StudentProgressRepository.CreateAsync(newProgress);
            await _unitOfWork.SaveChangesAsync();

            var createdProgress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLessonAsync(studentId, lessonId);

            return _mapper.Map<LessonProgressResponseDTO>(createdProgress);
        }
    
        public async Task<LessonProgressResponseDTO> UpdateLessonProgressAsync(
            Guid studentId, Guid lessonId, int completionPercentage, bool isCompleted)
        {
            if (completionPercentage < 0 || completionPercentage > 100)
            {
                throw new BadRequestException("CompletionPercentage phải từ 0 đến 100");
            }

            var progress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLessonAsync(studentId, lessonId);

            if (progress == null)
            {
                await StartLessonAsync(studentId, lessonId);
                progress = await _unitOfWork.StudentProgressRepository
                    .GetProgressByStudentAndLessonAsync(studentId, lessonId);
            }

            progress.CompletionPercentage = completionPercentage;
            progress.IsCompleted = isCompleted;
            progress.LastAccessedAt = DateTime.UtcNow;

            if (isCompleted && progress.CompletedAt == null)
            {
                progress.CompletedAt = DateTime.UtcNow;
                progress.Status = "completed";
                progress.CompletionPercentage = 100;
            }
            else if (isCompleted)
            {
                progress.Status = "completed";
                progress.CompletionPercentage = 100;
            }
            else
            {
                progress.Status = completionPercentage > 0 ? "in_progress" : "in_progress";
            }

            //_unitOfWork.StudentProgressRepository.Update(progress);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonProgressResponseDTO>(progress);
        }
        public async Task<ChapterProgressResponseDTO> GetProgressByChapterAsync(Guid studentId, Guid chapterId)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(chapterId);
            if (chapter == null)
                throw new NotFoundException("Chapter", chapterId);

            var allLessons = await _unitOfWork.LessonRepository
                .FindByCondition(l => l.ChapterId == chapterId, false)
                .ToListAsync();

            if (allLessons.Count == 0)
            {
                return new ChapterProgressResponseDTO
                {
                    ChapterId = chapter.ChapterId,
                    ChapterName = chapter.ChapterName,
                    TotalLessons = 0,
                    CompletedLessons = 0,
                    CompletionPercentage = 0,
                    LessonProgress = new List<LessonProgressResponseDTO>()
                };
            }

            var progressList = await _unitOfWork.StudentProgressRepository
                .GetProgressByChapterAsync(studentId, chapterId);

            var lessonProgressDTOs = allLessons.Select(lesson =>
            {
                var progress = progressList.FirstOrDefault(p => p.LessonId == lesson.LessonId);
                
                if (progress == null)
                {
                    return new LessonProgressResponseDTO
                    {
                        LessonId = lesson.LessonId,
                        LessonName = lesson.LessonName,
                        IsCompleted = false,
                        CompletionPercentage = 0,
                        EstimatedTime = lesson.EstimatedTime
                    };
                }

                return _mapper.Map<LessonProgressResponseDTO>(progress);
            }).ToList();

            int completedCount = lessonProgressDTOs.Count(l => l.IsCompleted);
            double avgCompletion = lessonProgressDTOs.Average(l => l.CompletionPercentage);

            return new ChapterProgressResponseDTO
            {
                ChapterId = chapter.ChapterId,
                ChapterName = chapter.ChapterName,
                TotalLessons = allLessons.Count,
                CompletedLessons = completedCount,
                CompletionPercentage = Math.Round(avgCompletion, 2),
                LessonProgress = lessonProgressDTOs
            };
        }
        public async Task<SubjectProgressResponseDTO> GetProgressBySubjectAsync(Guid studentId, Guid subjectId)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject", subjectId);

            var chapters = await _unitOfWork.ChapterRepository
                .FindByCondition(c => c.SubjectId == subjectId, false)
                .ToListAsync();

            if (chapters.Count == 0)
            {
                return new SubjectProgressResponseDTO
                {
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    TotalChapters = 0,
                    TotalLessons = 0,
                    CompletedChapters = 0,
                    CompletedLessons = 0,
                    CompletionPercentage = 0,
                    ChapterProgress = new List<ChapterProgressResponseDTO>()
                };
            }

            var chapterProgressList = new List<ChapterProgressResponseDTO>();

            foreach (var chapter in chapters)
            {
                var chapterProgress = await GetProgressByChapterAsync(studentId, chapter.ChapterId);
                chapterProgressList.Add(chapterProgress);
            }

            int totalLessons = chapterProgressList.Sum(c => c.TotalLessons);
            int completedLessons = chapterProgressList.Sum(c => c.CompletedLessons);
            int completedChapters = chapterProgressList.Count(c => c.CompletionPercentage == 100);

            double completionPercentage = totalLessons > 0
                ? Math.Round((double)completedLessons / totalLessons * 100, 2)
                : 0;

            return new SubjectProgressResponseDTO
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                TotalChapters = chapters.Count,
                TotalLessons = totalLessons,
                CompletedChapters = completedChapters,
                CompletedLessons = completedLessons,
                CompletionPercentage = completionPercentage,
                ChapterProgress = chapterProgressList
            };
        }
        public async Task<StudentProgressOverviewDTO> GetStudentProgressOverviewAsync(Guid studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student", studentId);

            var allSubjects = student.GradeLevel.HasValue
                ? await _unitOfWork.SubjectRepository
                    .FindByCondition(s => s.Grade.GradeLevel == student.GradeLevel.Value, false)
                    .ToListAsync()
                : await _unitOfWork.SubjectRepository
                    .FindByCondition(s => true, false)
                    .ToListAsync();

            var subjectProgressList = new List<SubjectProgressResponseDTO>();

            foreach (var subject in allSubjects)
            {
                var subjectProgress = await GetProgressBySubjectAsync(studentId, subject.SubjectId);
                subjectProgressList.Add(subjectProgress);
            }

            int totalLessons = subjectProgressList.Sum(s => s.TotalLessons);
            int completedLessons = subjectProgressList.Sum(s => s.CompletedLessons);

            var allProgress = await _unitOfWork.StudentProgressRepository
                .GetStudentProgressByStudentIdAsync(studentId);
            
            var lastActivity = allProgress.Any() 
                ? allProgress.Max(p => p.LastAccessedAt) 
                : null;

            var overallCompletion = totalLessons > 0
                ? (int)Math.Round((double)completedLessons / totalLessons * 100)
                : 0;

            // Calculate new stats
            var insights = await GetPerformanceInsightsAsync(studentId);
            var allGameResults = await _unitOfWork.GameResultRepository
                .FindByCondition(g => g.StudentId == studentId, false)
                .ToListAsync();

            var totalPoints = allGameResults.Sum(g => g.Score);

            return new StudentProgressOverviewDTO
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                GradeLevel = student.GradeLevel ?? 0,
                TotalSubjects = allSubjects.Count,
                TotalChapters = subjectProgressList.Sum(s => s.TotalChapters),
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                OverallCompletionPercentage = overallCompletion,
                LastActivityDate = lastActivity,
                Subjects = subjectProgressList,
                
                // New Stats
                LearningStreak = insights.LearningStreak,

                TotalPoints = totalPoints, 
                CurrentLevel = (completedLessons / 10) + 1 // Simple level calculation logic
            };
        }

        public async Task<IEnumerable<ParentStudentListDTO>> GetParentStudentListAsync(Guid parentId)
        {
            var parent = await _unitOfWork.UserRepository.GetByIdAsync(parentId);
            if (parent == null)
                throw new NotFoundException("Parent", parentId);

            // Lấy danh sách students trực tiếp từ Student table theo ParentId
            var students = await _unitOfWork.StudentRepository.GetStudentsByParentIdAsync(parentId);

            if (students == null || !students.Any())
            {
                return new List<ParentStudentListDTO>(); // Không có học sinh nào
            }

            var result = new List<ParentStudentListDTO>();

            foreach (var student in students)
            {
                var overview = await GetStudentProgressOverviewAsync(student.StudentId);

                result.Add(new ParentStudentListDTO
                {
                    StudentId = student.StudentId,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    GradeLevel = student.GradeLevel ?? 0,
                    AvatarUrl = student.AvatarUrl,
                    OverallCompletionPercentage = overview.OverallCompletionPercentage,
                    LastActivityDate = overview.LastActivityDate
                });
            }

            return result;
        }

        public async Task<bool> ValidateParentAccessAsync(Guid parentId, Guid studentId)
        {
            return await _unitOfWork.StudentProgressRepository.CanParentAccessStudentProgressAsync(parentId, studentId);
        }

        public async Task<IEnumerable<RecentActivityResponseDTO>> GetRecentActivitiesAsync(Guid studentId, int limit = 20, DateTime? startDate = null, DateTime? endDate = null)
        {
            // 1. Get Lesson activities from StudentProgress
            var lessonActivitiesList = await _unitOfWork.StudentProgressRepository.GetRecentProgressAsync(studentId, limit, startDate, endDate);
            
            var lessonActivities = lessonActivitiesList.Select(x => new RecentActivityResponseDTO
                {
                    ActivityId = x.StudentProgressId,
                    ActivityName = x.Lesson.LessonName,
                    ActivityType = "Lesson",
                    ReferenceId = x.LessonId,
                    ActivityDate = x.LastAccessedAt.Value,
                    DurationMinutes = x.CompletedAt.HasValue && x.StartedAt.HasValue 
                        ? (int)(x.CompletedAt.Value - x.StartedAt.Value).TotalMinutes 
                        : (x.CompletionPercentage ?? 0) / 5, // Estimate if not completed
                    Score = x.CompletionPercentage ?? 0,
                    Status = x.Status,
                    CorrectAnswers = null,
                    TotalQuestions = null
                }).ToList();

            // 2. Get Game activities from GameResult
            var gameActivitiesList = await _unitOfWork.GameResultRepository.GetRecentGameResultsAsync(studentId, limit, startDate, endDate);

            var gameActivities = gameActivitiesList.Select(x => new RecentActivityResponseDTO
                {
                    ActivityId = x.GameResultId,
                    ActivityName = x.Game.Name,
                    ActivityType = "Game",
                    ReferenceId = x.GameId,
                    ActivityDate = x.PlayedAt,
                    DurationMinutes = x.PlayDuration / 60,
                    Score = (double)x.Score,
                    Status = "Completed",
                    CorrectAnswers = x.CorrectAnswers,
                    TotalQuestions = x.TotalQuestions
                }).ToList();

            // 3. Merge and sort
            var allActivities = lessonActivities.Concat(gameActivities)
                .OrderByDescending(x => x.ActivityDate)
                .Take(limit)
                .ToList();

            return allActivities;
        }

        public async Task<PerformanceInsightResponseDTO> GetPerformanceInsightsAsync(Guid studentId)
        {
            var response = new PerformanceInsightResponseDTO();

            // 1. Calculate Learning Streak
            response.LearningStreak = await CalculateStreakAsync(studentId);

            // 2. Game Performance Stats
            var gameResults = await _unitOfWork.GameResultRepository
                .FindByCondition(x => x.StudentId == studentId, false)
                .Include(x => x.Game)
                .ToListAsync();

            response.TotalGamesPlayed = gameResults.Count;
            response.AverageGameScore = gameResults.Any() ? Math.Round(gameResults.Average(x => (double)x.Score), 1) : 0;

            // Chi phan tich khi co it nhat 5 lan choi game            
            if (gameResults.Count >= 5)
            {
                // lay 3 game gan nhat va 3 game cu nhat
                var recentAvg = gameResults.TakeLast(3).Average(x => (double)x.Score);
                var olderAvg = gameResults.Take(3).Average(x => (double)x.Score);
                // Nếu điểm gần đây cao hơn điểm trước thì là "Improving", ngược lại là "Declining", còn lại là "Stable"
                response.PerformanceTrend = recentAvg > olderAvg ? "Improving" : (recentAvg < olderAvg - 10 ? "Declining" : "Stable");
            }
            else
            {
                response.PerformanceTrend = "Stable";
            }

            if (response.AverageGameScore > 80)
            {
                response.Strengths.Add("Tư duy logic tốt");
                response.Strengths.Add("Hoàn thành bài tập nhanh");
                response.SuggestedFocus.Add("Thử thách với các bài toán nâng cao");
            }
            else if (response.AverageGameScore < 50) 
            {
                response.Weaknesses.Add("Cần cải thiện độ chính xác");
                response.SuggestedFocus.Add("Ôn tập lại các bài cơ bản");
            }
            else
            {
                response.Strengths.Add("Tiến bộ đều đặn");
                response.SuggestedFocus.Add("Duy trì thói quen học tập hàng ngày");
            }

            return response;
        }

        public async Task<Dictionary<DateTime, int>> GetStudyTimeStatisticsAsync(Guid studentId, DateTime startDate, DateTime endDate)
        {
            var result = new Dictionary<DateTime, int>();

            // Khởi tạo từ ngày bắt đầu đến ngày kết thúc với giá trị 0
            for (var dt = startDate.Date; dt <= endDate.Date; dt = dt.AddDays(1))
            {
                result[dt] = 0;
            }

            // Cộng dồn thời gian học từ bài học
            var lessonProgress = await _unitOfWork.StudentProgressRepository
                .GetProgressByDateRangeAsync(studentId, startDate, endDate);

            foreach (var p in lessonProgress)
            {
                if (p.LastAccessedAt.HasValue)
                {
                    var date = p.LastAccessedAt.Value.Date;
                    if (result.ContainsKey(date))
                    {
                        // Estimate 15 mins per lesson interaction if duration not tracked
                        int duration = p.CompletedAt.HasValue && p.StartedAt.HasValue 
                            ? (int)(p.CompletedAt.Value - p.StartedAt.Value).TotalMinutes 
                            : 15;
                        result[date] += duration;
                    }
                }
            }

            // Add game times
            var gameResults = await _unitOfWork.GameResultRepository
                .GetGameResultsByDateRangeAsync(studentId, startDate, endDate);

            foreach (var g in gameResults)
            {
                 var date = g.PlayedAt.Date;
                 if (result.ContainsKey(date))
                 {
                     result[date] += g.PlayDuration / 60; // Convert seconds to minutes
                 }
            }

            return result;
        }

        private async Task<int> CalculateStreakAsync(Guid studentId)
        {
            // Get lesson dates
            var lessonDates = await _unitOfWork.StudentProgressRepository.GetDistinctLearningDatesAsync(studentId);

            // Get game dates
            var gameDates = await _unitOfWork.GameResultRepository.GetDistinctPlayDatesAsync(studentId);

            // Merge and sort
            var allDates = lessonDates.Concat(gameDates)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            if (!allDates.Any()) return 0;

            int streak = 0;
            var checkDate = DateTime.Today;

            // Allow streak to continue if activity was yesterday (today no activity yet, but streak not broken)
            if (!allDates.Contains(checkDate))
            {
                checkDate = checkDate.AddDays(-1);
            }

            foreach (var date in allDates)
            {
                if (date.Date == checkDate)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else if (date.Date > checkDate)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }
    }
}
