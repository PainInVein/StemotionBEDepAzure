using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService _chapterService;

        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Chapter trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllChapter([FromQuery]PaginationRequestDTO requestDTO)
        {
            var result = await _chapterService.GetAllChapter(requestDTO);
            return Ok(ResponseDTO<PaginatedResponseDTO<ChapterResponseDTO>>.Success(result, "Lấy danh sách thành công"));
        }

        [EndpointDescription("API này lấy Chapter theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterByIdAsync(Guid id)
        {
            var chapter = await _chapterService.GetChapterById(id);
            return Ok(ResponseDTO<ChapterResponseDTO>.Success(chapter, "Lấy thông tin chương học thành công"));
        }

        [EndpointDescription("API này để tạo Chapter")]
        [HttpPost]
        public async Task<IActionResult> CreateChapter( ChapterRequestDTO createChapterRequest)
        {
            var result = await _chapterService.CreateChapter(createChapterRequest);
            return Ok(ResponseDTO<ChapterResponseDTO>.Success(result, "Tạo chương học thành công"));
        }

        [EndpointDescription("API này để sửa Chapter theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(Guid id, [FromBody] UpdateChapterRequestDTO updateChapterRequest)
        {
            var result = await _chapterService.UpdateChapter(id, updateChapterRequest);
            return Ok(ResponseDTO<ChapterResponseDTO>.Success(result, "Cập nhật chương học thành công"));
        }

        [EndpointDescription("API này để xóa Chapter theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var result = await _chapterService.DeleteChapter(id);
            return Ok(ResponseDTO<bool>.Success(result, "Xóa chương học thành công"));
        }

        [HttpGet("/get-by-chapter/{subjectName}")]
        public async Task<IActionResult> GetSubjectByGrade([FromQuery] PaginationRequestDTO requestDTO, string subjectName)
        {
            var result = await _chapterService.GetChapterBySubjectName(requestDTO, subjectName);
            return Ok(ResponseDTO<PaginatedResponseDTO<ChapterResponseDTO>>.Success(result, "Tìm thấy  thành công"));
        }
    }
}
