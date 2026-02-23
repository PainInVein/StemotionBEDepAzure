using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IGameResultService
    {
        Task<GameResultResponseDTO> SubmitResultAsync(Guid studentId, SubmitGameResultRequestDto dto);

        Task<IEnumerable<HistoryGameResultDto>> GetStudentResultAsync(Guid studentId);

        Task<IEnumerable<HistoryGameResultDto>> GetResultsByGameAsync(Guid studentId, Guid gameId);

        Task<StudentGameStatsResponseDTO> GetStudentGameStatsAsync(Guid studentId, Guid gameId);
        Task<GameResultResponseDTO> GetBestResultAsync(Guid studentId, Guid gameId);
        Task<int> GetAttemptCountAsync(Guid studentId, Guid gameId);
        Task<IEnumerable<StudentLeaderboardDTO>> GetLeaderboardAsync(int limit);
    }
}
