using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IGameResultRepository : IGenericRepository<GameResult>
    {


        Task<int> GetAttemptCountAsync(Guid studentId, Guid gameId);
        Task<GameResult?> GetBestScoreAsync(Guid studentId, Guid gameId);
        Task<IEnumerable<GameResult>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<GameResult>> GetByGameAndStudentAsync(Guid studentId, Guid gameId);
        Task<IEnumerable<GameResult>> GetRecentGameResultsAsync(Guid studentId, int limit, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<GameResult>> GetGameResultsByDateRangeAsync(Guid studentId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<DateTime>> GetDistinctPlayDatesAsync(Guid studentId);
        Task<IEnumerable<StudentLeaderboardDTO>> GetLeaderboardAsync(int limit);
    }
}
