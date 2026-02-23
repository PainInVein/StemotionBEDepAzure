using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IGameService
    {
        Task<GameResponseDTO> CreateGameAsync(CreateGameDTO requestDto);
        Task<GameResponseDTO> UpdateGameAsync(Guid gameId, UpdateGameDto dto);
        Task<bool> DeleteGameAsync(Guid gameId);
        Task<GameResponseDTO?> GetGameByIdAsync(Guid gameId);
        Task<IEnumerable<GameResponseDTO>> GetGameByLessonIdAsync(Guid lessionId);
        Task<IEnumerable<GameResponseDTO>> GetAllActiveGameAsync();
        Task<PaginatedResponseDTO<GameResponseDTO>> GetAllGames(PaginationRequestDTO requestDTO);
    }
}
