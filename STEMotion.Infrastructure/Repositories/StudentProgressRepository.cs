using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class StudentProgressRepository : GenericRepository<StudentProgress>, IStudentProgressRepository
    {
        private readonly StemotionContext _context;
        public StudentProgressRepository(StemotionContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách các bài học đã học của sinh viên trong một chương
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <param name="chapterId">ID của chương</param>
        /// <returns>Danh sách các bài học đã học của sinh viên trong một chương</returns>
        public async Task<IEnumerable<StudentProgress>> GetProgressByChapterAsync(Guid studentId, Guid chapterId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.ChapterId == chapterId, false)
                         .Include(x => x.Lesson)
                         .ToListAsync();
        }

        /// <summary>
        /// Lấy tiến độ học của sinh viên trong một bài học
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <param name="lessonId">ID của bài học</param>
        /// <returns>Tiến độ học của sinh viên trong một bài học</returns>
        public async Task<StudentProgress?> GetProgressByStudentAndLessonAsync(Guid studentId, Guid lessonId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.LessonId == lessonId, true)
                         .Include(x => x.Lesson)
                         .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy danh sách các bài học đã học của sinh viên trong một môn học
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <param name="subjectId">ID của môn học</param>
        /// <returns>Danh sách các bài học đã học của sinh viên trong một môn học</returns>
        public async Task<IEnumerable<StudentProgress>> GetProgressBySubjectAsync(Guid studentId, Guid subjectId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.Chapter.SubjectId == subjectId, false)
                        .Include(x => x.Lesson)
                                .ThenInclude(l => l.Chapter)
                        .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách các bài học đã học của sinh viên
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <returns>Danh sách các bài học đã học của sinh viên</returns>
        public async Task<IEnumerable<StudentProgress>> GetStudentProgressByStudentIdAsync(Guid studentId)
        {
            return await FindByCondition(x => x.StudentId == studentId, false)
                         .Include(x => x.Lesson)
                                 .ThenInclude(x => x.Chapter)
                                 .ThenInclude(x => x.Subject)
                                 .ThenInclude(x => x.Grade)
                         .ToListAsync();
        }

        /// <summary>
        /// Kiểm tra xem phụ huynh có quyền truy cập tiến độ học của sinh viên không
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của sinh viên</param>
        /// <returns>True nếu phụ huynh có quyền truy cập, ngược lại false</returns>
        public async Task<bool> CanParentAccessStudentProgressAsync(Guid parentId, Guid studentId)
        {
            // Check if student exists and belongs to the parent
            var student = await _context.Students
                .Where(s => s.StudentId == studentId && s.ParentId == parentId)
                .FirstOrDefaultAsync();

            return student != null;
        }

        /// <summary>
        /// Lấy danh sách các bài học đã học gần đây của sinh viên
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <param name="limit">Số lượng bài học cần lấy</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách các bài học đã học gần đây của sinh viên</returns>
        public async Task<IEnumerable<StudentProgress>> GetRecentProgressAsync(Guid studentId, int limit, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<StudentProgress> query = FindByCondition(x => x.StudentId == studentId && x.LastAccessedAt.HasValue, false)
                .Include(x => x.Lesson);

            if (startDate.HasValue)
                query = query.Where(x => x.LastAccessedAt >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(x => x.LastAccessedAt <= endDate.Value);

            return await query
                .OrderByDescending(x => x.LastAccessedAt)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách các bài học đã học của sinh viên trong một khoảng thời gian
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <returns>Danh sách các bài học đã học của sinh viên trong một khoảng thời gian</returns>
        public async Task<IEnumerable<StudentProgress>> GetProgressByDateRangeAsync(Guid studentId, DateTime startDate, DateTime endDate)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.LastAccessedAt >= startDate && x.LastAccessedAt <= endDate, false)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách các ngày học của sinh viên
        /// </summary>
        /// <param name="studentId">ID của sinh viên</param>
        /// <returns>Danh sách các ngày học của sinh viên</returns>
        public async Task<IEnumerable<DateTime>> GetDistinctLearningDatesAsync(Guid studentId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.LastAccessedAt.HasValue, false)
                .Select(x => x.LastAccessedAt.Value.Date)
                .Distinct()
                .ToListAsync();
        }
    }
}
