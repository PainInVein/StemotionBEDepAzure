using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
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
    public class GameResultService : IGameResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameResultService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetAttemptCountAsync(Guid studentId, Guid gameId)
        {
            return await _unitOfWork.GameResultRepository.GetAttemptCountAsync(studentId, gameId);
        }

        public async Task<GameResultResponseDTO> GetBestResultAsync(Guid studentId, Guid gameId)
        {
            var bestResult = await _unitOfWork.GameResultRepository.GetBestScoreAsync(studentId, gameId);

            if (bestResult == null)
                return null;

            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);

            var response = _mapper.Map<GameResultResponseDTO>(bestResult);
            response.GameName = game?.Name;

            return response;
        }


        // 2.
        public async Task<IEnumerable<HistoryGameResultDto>> GetResultsByGameAsync(Guid studentId, Guid gameId)
        {
            var historyGame = await _unitOfWork.GameResultRepository.FindByCondition(x => x.StudentId == studentId && x.GameId == gameId)
                .Include(x => x.Game)
                .OrderByDescending(x => x.PlayedAt)
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<HistoryGameResultDto>>(historyGame);
            return result;
        }

        // 1.
        public async Task<StudentGameStatsResponseDTO> GetStudentGameStatsAsync(Guid studentId, Guid gameId)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new NotFoundException("Không tìm thấy game");
            }

            var attemptCount = await _unitOfWork.GameResultRepository.GetAttemptCountAsync(studentId, gameId);

            var bestResult = await _unitOfWork.GameResultRepository.GetBestScoreAsync(studentId, gameId);

            var allResults = await _unitOfWork.GameResultRepository
                                              .FindByCondition(x => x.StudentId == studentId && x.GameId == gameId)
                                              .OrderByDescending(x => x.PlayedAt)
                                              .ToListAsync();

            var lastPlayed = allResults.FirstOrDefault();

            var stats = bestResult != null ? _mapper.Map<StudentGameStatsResponseDTO>(bestResult) : new StudentGameStatsResponseDTO
            {
                GameId = gameId,
                GameName = game.Name,
            };

            stats.AttemptCount = attemptCount;
            stats.LastPlayedAt = lastPlayed?.PlayedAt;

            return stats;
        }

        public async Task<IEnumerable<HistoryGameResultDto>> GetStudentResultAsync(Guid studentId)
        {
            var student = await _unitOfWork.GameResultRepository.GetByStudentIdAsync(studentId);
            var result = _mapper.Map<IEnumerable<HistoryGameResultDto>>(student);
            return result;
        }

        public async Task<GameResultResponseDTO> SubmitResultAsync(Guid studentId, SubmitGameResultRequestDto dto)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(dto.GameId);
            if (game == null)
            {
                throw new NotFoundException("Không tìm thấy game");
            }

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
            {
                throw new NotFoundException("Không tìm thấy học sinh");
            }

            var gameResult = _mapper.Map<GameResult>(dto);
            gameResult.StudentId = studentId;
            gameResult.PlayedAt = DateTime.Now;

            await _unitOfWork.GameResultRepository.CreateAsync(gameResult);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<GameResultResponseDTO>(gameResult);
            result.GameName = game.Name;

            return result;
        }

        public async Task<IEnumerable<StudentLeaderboardDTO>> GetLeaderboardAsync(int limit)
        {
            return await _unitOfWork.GameResultRepository.GetLeaderboardAsync(limit);
        }
    }
}
