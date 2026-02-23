using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProgressController : ControllerBase
    {
        private readonly IStudentProgressService _studentProgressService;

        public StudentProgressController(IStudentProgressService studentProgressService)
        {
            _studentProgressService = studentProgressService;
        }

        [HttpGet("lesson/{lessonId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ của một lesson cụ thể")]
        public async Task<IActionResult> GetLessonProgress(Guid studentId, Guid lessonId)
        {
            var result = await _studentProgressService.GetLessonProgressAsync(studentId, lessonId);
            
            if (result == null)
            {
                return Ok(ResponseDTO<object>.Success(null, "Lesson chưa được bắt đầu"));
            }

            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Lấy tiến độ lesson thành công"));
        }

        [HttpPost("lesson/{lessonId}/student/{studentId}/start")]
        [EndpointDescription("Bắt đầu học một lesson")]
        public async Task<IActionResult> StartLesson(Guid studentId, Guid lessonId)
        {
            var result = await _studentProgressService.StartLessonAsync(studentId, lessonId);
            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Bắt đầu lesson thành công"));
        }

        [HttpPut("lesson/{lessonId}/student/{studentId}")]
        [EndpointDescription("Cập nhật tiến độ lesson")]
        public async Task<IActionResult> UpdateLessonProgress(
            Guid studentId, 
            Guid lessonId,
            [FromBody] UpdateProgressRequest request)
        {
            var result = await _studentProgressService.UpdateLessonProgressAsync(
                studentId, lessonId, request.CompletionPercentage, request.IsCompleted);
            
            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Cập nhật tiến độ thành công"));
        }

        [HttpGet("chapter/{chapterId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ theo chapter")]
        public async Task<IActionResult> GetProgressByChapter(Guid studentId, Guid chapterId)
        {
            var result = await _studentProgressService.GetProgressByChapterAsync(studentId, chapterId);
            return Ok(ResponseDTO<ChapterProgressResponseDTO>.Success(result, "Lấy tiến độ chapter thành công"));
        }

        [HttpGet("subject/{subjectId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ theo subject")]
        public async Task<IActionResult> GetProgressBySubject(Guid studentId, Guid subjectId)
        {
            var result = await _studentProgressService.GetProgressBySubjectAsync(studentId, subjectId);
            return Ok(ResponseDTO<SubjectProgressResponseDTO>.Success(result, "Lấy tiến độ subject thành công"));
        }

        [HttpGet("student/{studentId}/overview")]
        [EndpointDescription("Lấy tổng quan tiến độ học tập của học sinh")]
        public async Task<IActionResult> GetStudentOverview(Guid studentId)
        {
            var result = await _studentProgressService.GetStudentProgressOverviewAsync(studentId);
            return Ok(ResponseDTO<StudentProgressOverviewDTO>.Success(result, "Lấy tổng quan thành công"));
        }

        [HttpGet("student/{studentId}/recent-activities")]
        [EndpointDescription("Lấy danh sách hoạt động gần đây")]
        public async Task<IActionResult> GetRecentActivities(
            Guid studentId, 
            [FromQuery] int limit = 20,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            // Get ParentId from authenticated user claims
            // Assuming the User ID in token is the Parent's ID
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid parentId))
            {
                var hasAccess = await _studentProgressService.ValidateParentAccessAsync(parentId, studentId);
                if (!hasAccess)
                {
                    return Forbid();
                }
            }

            var result = await _studentProgressService.GetRecentActivitiesAsync(studentId, limit, startDate, endDate);
            return Ok(ResponseDTO<IEnumerable<RecentActivityResponseDTO>>.Success(result, "Lấy danh sách hoạt động thành công"));
        }

        [HttpGet("student/{studentId}/insights")]
        [EndpointDescription("Lấy phân tích hiệu suất học tập")]
        public async Task<IActionResult> GetPerformanceInsights(Guid studentId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid parentId))
            {
                var hasAccess = await _studentProgressService.ValidateParentAccessAsync(parentId, studentId);
                if (!hasAccess)
                {
                    return Forbid();
                }
            }

            var result = await _studentProgressService.GetPerformanceInsightsAsync(studentId);
            return Ok(ResponseDTO<PerformanceInsightResponseDTO>.Success(result, "Lấy phân tích hiệu suất thành công"));
        }

        [HttpGet("student/{studentId}/study-time")]
        [EndpointDescription("Lấy thống kê thời gian học tập theo ngày")]
        public async Task<IActionResult> GetStudyTimeStatistics(
            Guid studentId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid parentId))
            {
                var hasAccess = await _studentProgressService.ValidateParentAccessAsync(parentId, studentId);
                if (!hasAccess)
                {
                    return Forbid();
                }
            }

            var result = await _studentProgressService.GetStudyTimeStatisticsAsync(studentId, startDate, endDate);
            return Ok(ResponseDTO<Dictionary<DateTime, int>>.Success(result, "Lấy thống kê thời gian thành công"));
        }

        [HttpGet("parent/{parentId}/students")]
        [EndpointDescription("Lấy danh sách học sinh của phụ huynh")]
        public async Task<IActionResult> GetParentStudents(Guid parentId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid tokenUserId))
            {
                if (tokenUserId != parentId)
                {
                    return Forbid();
                }
            }

            var result = await _studentProgressService.GetParentStudentListAsync(parentId);
            return Ok(ResponseDTO<IEnumerable<ParentStudentListDTO>>.Success(result, "Lấy danh sách thành công"));
        }
    }
}
