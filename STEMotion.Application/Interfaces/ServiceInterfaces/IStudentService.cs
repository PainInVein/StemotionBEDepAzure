using STEMotion.Application.DTO.RequestDTOs.StudentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs.StudentResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IStudentService
    {
        Task<AddChildrenResponseDTO> AddStudentAsync(AddChildrenRequestDTO addChildrenRequestDTO);
        Task<StudentLoginResonseDTO?> LoginStudent(StudentLoginRequestDTO studentLoginRequest);
    }
}
