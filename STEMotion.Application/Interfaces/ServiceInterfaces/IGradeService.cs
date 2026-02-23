using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IGradeService
    {
        Task<PaginatedResponseDTO<GradeResponseDTO>> GetAllGrade(PaginationRequestDTO requestDTO);
        Task<GradeResponseDTO> GetGradeById(Guid id);
        Task<GradeResponseDTO> CreateGrade(GradeRequestDTO requestDTO);
        Task<GradeResponseDTO> UpdateGrade(Guid id, UpdateGradeRequest requestDTO);
        Task<bool> DeleteGrade(Guid id);
    }
}
