using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Subject trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllSubject([FromQuery] PaginationRequestDTO requestDTO)
        {
            var result = await _subjectService.GetAllSubject(requestDTO);
            return Ok(ResponseDTO<PaginatedResponseDTO<SubjectResponseDTO>>.Success(result, "Lấy danh sách môn học thành công"));
        }

        [EndpointDescription("API này lấy Subject theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectByIdAsync(Guid id)
        {
            var result = await _subjectService.GetSubjectById(id);
            return Ok(ResponseDTO<SubjectResponseDTO>.Success(result, "Tìm thấy thông tin môn học"));
        }

        [EndpointDescription("API này để tạo Subject")]
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectRequestDTO createSubjectRequest)
        {
            var result = await _subjectService.CreateSubject(createSubjectRequest);
            return Ok(ResponseDTO<SubjectResponseDTO>.Success(result, "Tạo môn học thành công"));
        }

        [EndpointDescription("API này để sửa Subject theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] UpdateSubjectRequestDTO updateSubjectRequest)
        {
            var result = await _subjectService.UpdateSubject(id, updateSubjectRequest);
            return Ok(ResponseDTO<SubjectResponseDTO>.Success(result, "Cập nhật môn học thành công"));
        }

        [EndpointDescription("API này để xóa Subject theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var result = await _subjectService.DeleteSubject(id);
            return Ok(ResponseDTO<bool>.Success(result, "Xóa môn học thành công"));
        }

        [HttpGet("/get-by-subject/{gradeLevel}")]
        public async Task<IActionResult> GetSubjectByGrade([FromQuery]PaginationRequestDTO requestDTO,int gradeLevel)
        {
            var result = await _subjectService.GetSubjectByGradeLevel(requestDTO,gradeLevel);
            return Ok(ResponseDTO<PaginatedResponseDTO<SubjectResponseDTO>>.Success(result, "Tìm thấy  thành công"));
        }
    }
}
