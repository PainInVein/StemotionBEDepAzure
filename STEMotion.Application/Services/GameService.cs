using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GameResponseDTO> CreateGameAsync(CreateGameDTO requestDto)
        {
            var existGameCode = await _unitOfWork.GameRepository.GameCodeExistsAsync(requestDto.GameCode);
            if(existGameCode)
            {
                throw new NotFoundException($"Game code {requestDto.GameCode} đã tồn tại");
            }

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(requestDto.LessonId);
            if (lesson == null)
            {
                throw new NotFoundException($"Không tìm thấy bài học.");
            }

            if (!string.IsNullOrEmpty(requestDto.ConfigData))
            {
                try
                {
                    JsonDocument.Parse(requestDto.ConfigData);
                }
                catch (JsonException ex)
                {
                    throw new ArgumentException("ConfigData phải là JSON hợp lệ");
                }
            }

            var game = _mapper.Map<Game>(requestDto);

            await _unitOfWork.GameRepository.CreateAsync(game);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<GameResponseDTO>(game);
            return result;
        }

        public async Task<bool> DeleteGameAsync(Guid gameId)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
            if (game == null)
                return false;

            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GameResponseDTO>> GetAllActiveGameAsync()
        {
            var games = await _unitOfWork.GameRepository
                                         .FindByCondition(x => x.Status == true)
                                         .Include(x => x.Lesson)
                                         .OrderBy(x => x.Name)
                                         .ToListAsync();
            var result = _mapper.Map<IEnumerable<GameResponseDTO>>(games);
            return result;
        }

        public async Task<PaginatedResponseDTO<GameResponseDTO>> GetAllGames(PaginationRequestDTO requestDTO)
        {

            var (game, total) = await _unitOfWork.GameRepository.GetPagedAsync(requestDTO.PageNumber, requestDTO.PageSize, null, x => x.Lesson);
            var response = _mapper.Map<IEnumerable<GameResponseDTO>>(game);
            return new PaginatedResponseDTO<GameResponseDTO>
            {
                Items = response,
                PageNumber = requestDTO.PageNumber,
                PageSize = requestDTO.PageSize,
                TotalCount = total
            };
        }

        public async Task<GameResponseDTO?> GetGameByIdAsync(Guid gameId)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
            if (game == null)
                return null;
            var result = _mapper.Map<GameResponseDTO>(game);
            return result;
        }

        public async Task<IEnumerable<GameResponseDTO>> GetGameByLessonIdAsync(Guid lessionId)
        {
            var games = await _unitOfWork.GameRepository.GetByLessonIdAsync(lessionId);
            var result = _mapper.Map<IEnumerable<GameResponseDTO>>(games);
            return result;
        }
        public async Task<GameResponseDTO> UpdateGameAsync(Guid gameId, UpdateGameDto dto)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new NotFoundException("Không tìm thấy game");
            }
            if (!string.IsNullOrEmpty(dto.ConfigData))
            {
                try
                {
                    JsonDocument.Parse(dto.ConfigData);
                }
                catch (JsonException ex)
                {
                    throw new ArgumentException("ConfigData phải là Json hợp lệ");
                }
            }

            _mapper.Map(dto, game);
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();
            return await GetGameByIdAsync(gameId);
        }
    }
}
