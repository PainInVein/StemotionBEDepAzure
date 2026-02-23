using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

namespace STEMotion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonContentController : ControllerBase
    {
        private readonly ILessonContentService _service;

        public LessonContentController(ILessonContentService service)
        {
            _service = service;
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonId(Guid lessonId)
        {
            var result = await _service.GetContentsByLessonId(lessonId);
            return Ok(ResponseDTO<IEnumerable<LessonContentResponseDTO>>.Success(result));
        }

        [HttpGet("by-name/{lessonName}")]
        public async Task<IActionResult> GetByLessonName(string lessonName)
        {
            var result = await _service.GetContentsByLessonName(lessonName);
            return Ok(ResponseDTO<IEnumerable<LessonContentResponseDTO>>.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            return Ok(ResponseDTO<LessonContentResponseDTO>.Success(result, "Lấy thông tin nội dung môn học thành công"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLessonContentRequestDTO request)
        {
            var result = await _service.Create(request);
            return Ok(ResponseDTO<LessonContentResponseDTO>.Success(result, "Tạo nội dung bài học thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonContentRequestDTO request)
        {
            var result = await _service.Update(id, request);
            return Ok(ResponseDTO<LessonContentResponseDTO>.Success(result, "Cập nhật khối lớp thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);
            return Ok(ResponseDTO<bool>.Success(result, "Xóa nội dung bài học thành công"));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] string newStatus)
        {
            var result = await _service.ChangeStatus(id, newStatus);
            return Ok(ResponseDTO<bool>.Success(result, "Thay đổi status nội dung bài học thành công"));
        }
    }
}