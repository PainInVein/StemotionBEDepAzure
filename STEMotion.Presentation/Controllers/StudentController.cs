using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs.StudentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.DTO.ResponseDTOs.StudentResponseDTO;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] AddChildrenRequestDTO addChildrenRequestDTO)
        {
            var result = await _studentService.AddStudentAsync(addChildrenRequestDTO);

            return Ok(ResponseDTO<AddChildrenResponseDTO>.Success(result, "Thêm học sinh thành công"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginStudent([FromBody] StudentLoginRequestDTO studentLoginRequest)
        {
            var result = await _studentService.LoginStudent(studentLoginRequest);
            if (result != null)
            {
                return Ok(ResponseDTO<StudentLoginResonseDTO>.Success(result, "Đăng nhập thành công"));
            }

            return BadRequest(ResponseDTO<string>.Fail(null!, "Đăng nhập thất bại"));
        }
    }
}
