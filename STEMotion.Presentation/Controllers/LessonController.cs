using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Lesson trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllLesson([FromQuery] PaginationRequestDTO requestDTO)
        {
            var result = await _lessonService.GetAllLesson(requestDTO);
            return Ok(ResponseDTO<PaginatedResponseDTO<LessonResponseDTO>>.Success(result, "Lấy danh sách bài học thành công"));
        }

        [EndpointDescription("API này lấy Lesson theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonByIdAsync(Guid id)
        {
            var result = await _lessonService.GetLessonById(id);
            return Ok(ResponseDTO<LessonResponseDTO>.Success(result, "Tìm thấy thông tin bài học"));
        }

        [EndpointDescription("API này để tạo Lesson")]
        [HttpPost]  
        public async Task<IActionResult> CreateLesson([FromBody] LessonRequestDTO createLessonRequest)
        {
            var result = await _lessonService.CreateLesson(createLessonRequest);
            return Ok(ResponseDTO<LessonResponseDTO>.Success(result, "Tạo bài học mới thành công"));
        }

        [EndpointDescription("API này để sửa Lesson theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdateLessonRequestDTO updateLessonRequest)
        {
            var result = await _lessonService.UpdateLesson(id, updateLessonRequest);
            return Ok(ResponseDTO<LessonResponseDTO>.Success(result, "Cập nhật bài học thành công"));
        }

        [EndpointDescription("API này để xóa Lesson theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var result = await _lessonService.DeleteLesson(id);
            return Ok(ResponseDTO<bool>.Success(result, "Xóa bài học thành công"));
        }

        [HttpGet("/get-by-lesson/{chapterName}")]
        public async Task<IActionResult> GetSubjectByGrade([FromQuery] PaginationRequestDTO requestDTO, string chapterName)
        {
            var result = await _lessonService.GetSubjectByChapterName(requestDTO, chapterName);
            return Ok(ResponseDTO<PaginatedResponseDTO<LessonResponseDTO>>.Success(result, "Tìm thấy  thành công"));
        }
    }
}
