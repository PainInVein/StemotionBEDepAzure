using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameResultsController : ControllerBase
    {
        private readonly IGameResultService _gameResultService;

        public GameResultsController(IGameResultService gameResultService)
        {
            _gameResultService = gameResultService;
        }

        [EndpointDescription("API submit kết quả chơi game")]
        [HttpPost]
        public async Task<IActionResult> SubmitResult([FromBody] SubmitGameResultRequestDto submitRequest)
        {
            var result = await _gameResultService.SubmitResultAsync(submitRequest.StudentId, submitRequest);
            return Ok(ResponseDTO<GameResultResponseDTO>.Success(result, "Lưu kết quả thành công"));
        }

        [EndpointDescription("API lấy lịch sử chơi game của học sinh")]
        [HttpGet("my-results")]
        public async Task<IActionResult> GetMyResults()
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _gameResultService.GetStudentResultAsync(studentId);
            return Ok(ResponseDTO<IEnumerable<HistoryGameResultDto>>.Success(result, "Lấy lịch sử thành công"));
        }

        [EndpointDescription("API lấy lịch sử chơi 1 game cụ thể")]
        [HttpGet("game/{gameId}")]
        public async Task<IActionResult> GetResultsByGame(Guid gameId)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _gameResultService.GetResultsByGameAsync(studentId, gameId);
            return Ok(ResponseDTO<IEnumerable<HistoryGameResultDto>>.Success(result, "Lấy lịch sử game thành công"));
        }

        [EndpointDescription("API lấy thống kê game của học sinh")]
        [HttpGet("stats/{gameId}")]
        public async Task<IActionResult> GetGameStats(Guid gameId)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _gameResultService.GetStudentGameStatsAsync(studentId, gameId);
            return Ok(ResponseDTO<StudentGameStatsResponseDTO>.Success(result, "Lấy thống kê thành công"));
        }

        [EndpointDescription("API lấy kết quả tốt nhất của học sinh cho 1 game")]
        [HttpGet("best/{gameId}")]
        public async Task<IActionResult> GetBestResult(Guid gameId)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _gameResultService.GetBestResultAsync(studentId, gameId);
            return Ok(ResponseDTO<GameResultResponseDTO>.Success(result, "Lấy kết quả tốt nhất thành công"));
        }

        [EndpointDescription("API lấy số lần đã chơi game")]
        [HttpGet("attempts/{gameId}")]
        public async Task<IActionResult> GetAttemptCount(Guid gameId)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _gameResultService.GetAttemptCountAsync(studentId, gameId);
            return Ok(ResponseDTO<int>.Success(result, "Lấy số lần chơi thành công"));
        }

        [EndpointDescription("API lấy bảng xếp hạng")]
        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int limit = 10)
        {
            var result = await _gameResultService.GetLeaderboardAsync(limit);
            return Ok(ResponseDTO<IEnumerable<StudentLeaderboardDTO>>.Success(result, "Lấy bảng xếp hạng thành công"));
        }
    }
}