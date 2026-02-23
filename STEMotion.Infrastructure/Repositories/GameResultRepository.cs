using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
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
    public class GameResultRepository : GenericRepository<GameResult>, IGameResultRepository
    {
        private readonly StemotionContext _context;
        public GameResultRepository(StemotionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetAttemptCountAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .CountAsync(x => x.StudentId == studentId && x.GameId == gameId);
        }

        public async Task<GameResult?> GetBestScoreAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .Where(x => x.StudentId == studentId && x.GameId == gameId)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.CorrectAnswers)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GameResult>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.GameResults
                .Include(x => x.Game)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.PlayedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameResult>> GetByGameAndStudentAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .Where(x => x.StudentId == studentId && x.GameId == gameId)
                .OrderByDescending(x => x.PlayedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameResult>> GetRecentGameResultsAsync(Guid studentId, int limit, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.GameResults
                .AsNoTracking()
                .Where(x => x.StudentId == studentId)
                .Include(x => x.Game)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.PlayedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.PlayedAt <= endDate.Value);

            return await query
                .OrderByDescending(x => x.PlayedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameResult>> GetGameResultsByDateRangeAsync(Guid studentId, DateTime startDate, DateTime endDate)
        {
            return await _context.GameResults
                .AsNoTracking()
                .Where(x => x.StudentId == studentId && x.PlayedAt >= startDate && x.PlayedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> GetDistinctPlayDatesAsync(Guid studentId)
        {
            return await _context.GameResults
                .AsNoTracking()
                .Where(x => x.StudentId == studentId)
                .Select(x => x.PlayedAt.Date)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentLeaderboardDTO>> GetLeaderboardAsync(int limit)
        {
            var leaderboard = await _context.GameResults
                .GroupBy(x => x.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    TotalScore = g.Sum(x => x.Score)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(limit)
                .Join(_context.Students,
                    stat => stat.StudentId,
                    student => student.StudentId,
                    (stat, student) => new StudentLeaderboardDTO
                    {
                        StudentId = student.StudentId,
                        StudentName = student.FirstName + ' ' + student.LastName,
                        AvatarUrl = student.AvatarUrl,
                        TotalScore = stat.TotalScore,
                        Rank = 0 // Will assign rank in memory if needed or here
                    })
                .ToListAsync();
            
            // Assign rank
            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }

            return leaderboard;
        }
    }
}
