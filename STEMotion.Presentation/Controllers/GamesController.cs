using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Threading.Tasks;

namespace STEMotion.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [EndpointDescription("API lấy thông tin game theo ID")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound(ResponseDTO<object>.Fail("Không tìm thấy game", "NOT_FOUND"));

            return Ok(ResponseDTO<GameResponseDTO>.Success(game, "Lấy thông tin game thành công"));
        }

        [EndpointDescription("API lấy danh sách game theo bài học")]
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetGamesByLesson(Guid lessonId)
        {
            var games = await _gameService.GetGameByLessonIdAsync(lessonId);
            return Ok(ResponseDTO<IEnumerable<GameResponseDTO>>.Success(games, "Lấy danh sách game thành công"));
        }

        [EndpointDescription("API lấy tất cả game đang hoạt động (Status là Active)")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveGames()
        {
            var games = await _gameService.GetAllActiveGameAsync();
            return Ok(ResponseDTO<IEnumerable<GameResponseDTO>>.Success(games, "Lấy danh sách game hoạt động thành công"));
        }

        [EndpointDescription("API lấy tất cả game với phân trang")]
        [HttpGet]
        public async Task<IActionResult> GetAllGames([FromQuery] PaginationRequestDTO requestDto)
        {
            var result = await _gameService.GetAllGames(requestDto);
            return Ok(ResponseDTO<PaginatedResponseDTO<GameResponseDTO>>.Success(result, "Lấy danh sách game thành công"));
        }

        [HttpPost]
        [EndpointDescription("API tạo game mới")]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseDTO<object>.Fail("Dữ liệu không hợp lệ", "VALIDATION_ERROR", ModelState));

            var game = await _gameService.CreateGameAsync(dto);
            return CreatedAtAction(
                nameof(GetGameById),
                new { id = game.GameId },
                ResponseDTO<GameResponseDTO>.Success(game, "Tạo game thành công")
            );
        }


        [HttpPut("{id}")]
        [EndpointDescription("API cập nhật game theo ID")]
        public async Task<IActionResult> UpdateGame(Guid id, [FromBody] UpdateGameDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseDTO<object>.Fail("Dữ liệu không hợp lệ", "VALIDATION_ERROR", ModelState));

            var game = await _gameService.UpdateGameAsync(id, dto);
            return Ok(ResponseDTO<GameResponseDTO>.Success(game, "Cập nhật game thành công"));
        }

        [HttpDelete("{id}")]
        [EndpointDescription("API xóa game theo ID")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success)
                return NotFound(ResponseDTO<object>.Fail("Không tìm thấy game", "NOT_FOUND"));

            return Ok(ResponseDTO<object>.Success(null, "Xóa game thành công"));
        }
    }
}