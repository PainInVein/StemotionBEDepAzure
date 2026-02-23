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
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Grade trong db")]
        // GET: api/<GradeController>
        [HttpGet]
        public async Task<IActionResult> GetAllGrade([FromQuery] PaginationRequestDTO requestDTO)
        {
            var result = await _gradeService.GetAllGrade(requestDTO);
            return Ok(ResponseDTO<PaginatedResponseDTO<GradeResponseDTO>>.Success(result, "Lấy danh sách khối lớp thành công"));
        }

        [EndpointDescription("API này lấy Grade theo Id")]
        // GET api/<GradeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGradeByIdAsync(Guid id)
        {
            var result = await _gradeService.GetGradeById(id);
            return Ok(ResponseDTO<GradeResponseDTO>.Success(result, "Lấy thông tin khối lớp thành công"));
        }

        [EndpointDescription("API này để tạo grade")]
        // POST api/<GradeController>
        [HttpPost]
        public async Task<IActionResult> CreateGrade([FromBody] GradeRequestDTO createGradeRequest)
        {
            var result = await _gradeService.CreateGrade(createGradeRequest);
            return Ok(ResponseDTO<GradeResponseDTO>.Success(result, "Tạo khối lớp thành công"));
        }
        [EndpointDescription("API này để sửa Grade theo Id")]
        // PUT api/<GradeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(Guid id, [FromBody] UpdateGradeRequest updateGradeRequest)
        {
            var result = await _gradeService.UpdateGrade(id, updateGradeRequest);
            return Ok(ResponseDTO<GradeResponseDTO>.Success(result, "Cập nhật khối lớp thành công"));
        }

        [EndpointDescription("API này để xóa User theo Id")]
        // DELETE api/<GradeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            var result = await _gradeService.DeleteGrade(id);
            return Ok(ResponseDTO<bool>.Success(result, "Xóa khối lớp thành công"));
        }
    }
}
