using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ILessonService
    {
        Task<PaginatedResponseDTO<LessonResponseDTO>> GetAllLesson(PaginationRequestDTO requestDTO);
        Task<LessonResponseDTO> GetLessonById(Guid id);
        Task<LessonResponseDTO> CreateLesson(LessonRequestDTO requestDTO);
        Task<LessonResponseDTO> UpdateLesson(Guid id, UpdateLessonRequestDTO requestDTO);
        Task<bool> DeleteLesson(Guid id);

        Task<PaginatedResponseDTO<LessonResponseDTO>> GetSubjectByChapterName(PaginationRequestDTO requestDTO, string chapterName);

    }
}
