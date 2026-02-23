using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IStudentProgressService
    {
        /// <summary>
        /// Lấy tổng quan tiến độ học tập của học sinh
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <returns>Thông tin tổng quan tiến độ</returns>
        Task<StudentProgressOverviewDTO> GetStudentProgressOverviewAsync(Guid studentId);

        /// <summary>
        /// Lấy tiến độ học tập của học sinh theo môn học
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="subjectId">ID của môn học</param>
        /// <returns>Thông tin tiến độ theo môn học</returns>
        Task<SubjectProgressResponseDTO> GetProgressBySubjectAsync(Guid studentId, Guid subjectId);

        /// <summary>
        /// Lấy tiến độ học tập của học sinh theo chương
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="chapterId">ID của chương</param>
        /// <returns>Thông tin tiến độ theo chương</returns>
        Task<ChapterProgressResponseDTO> GetProgressByChapterAsync(Guid studentId, Guid chapterId);

        /// <summary>
        /// Lấy danh sách học sinh của phụ huynh với tiến độ tổng quan
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <returns>Danh sách học sinh kèm tiến độ tổng quan</returns>
        Task<IEnumerable<ParentStudentListDTO>> GetParentStudentListAsync(Guid parentId);

        /// <summary>
        /// Cập nhật tiến độ học tập của học sinh cho một bài học
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="lessonId">ID của bài học</param>
        /// <param name="completionPercentage">Phần trăm hoàn thành</param>
        /// <param name="isCompleted">Đã hoàn thành hay chưa</param>
        /// <returns>Thông tin tiến độ đã cập nhật</returns>
        Task<LessonProgressResponseDTO> UpdateLessonProgressAsync(Guid studentId, Guid lessonId, int completionPercentage, bool isCompleted);

        /// <summary>
        /// Bắt đầu một bài học (ghi nhận thời gian bắt đầu)
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="lessonId">ID của bài học</param>
        /// <returns>Thông tin tiến độ bài học</returns>
        Task<LessonProgressResponseDTO> StartLessonAsync(Guid studentId, Guid lessonId);

        /// <summary>
        /// Lấy tiến độ của một bài học cụ thể
        /// </summary>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="lessonId">ID của bài học</param>
        /// <returns>Thông tin tiến độ bài học</returns>
        Task<LessonProgressResponseDTO> GetLessonProgressAsync(Guid studentId, Guid lessonId);

        /// <summary>
        /// Kiểm tra phụ huynh có quyền xem tiến độ của học sinh hay không
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của học sinh</param>
        /// <returns>True nếu có quyền, False nếu không</returns>
        Task<bool> ValidateParentAccessAsync(Guid parentId, Guid studentId);

        /// <summary>
        /// Lấy danh sách hoạt động gần đây (bài học và trò chơi)
        /// </summary>
        Task<IEnumerable<RecentActivityResponseDTO>> GetRecentActivitiesAsync(Guid studentId, int limit = 20, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Lấy phân tích hiệu suất và đề xuất học tập
        /// </summary>
        Task<PerformanceInsightResponseDTO> GetPerformanceInsightsAsync(Guid studentId);

        /// <summary>
        /// Lấy thống kê thời gian học tập theo ngày
        /// </summary>
        Task<Dictionary<DateTime, int>> GetStudyTimeStatisticsAsync(Guid studentId, DateTime startDate, DateTime endDate);
    }
}