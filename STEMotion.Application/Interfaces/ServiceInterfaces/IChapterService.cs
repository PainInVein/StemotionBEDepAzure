using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IChapterService
    {
        Task<PaginatedResponseDTO<ChapterResponseDTO>> GetAllChapter(PaginationRequestDTO requestDTO);
        Task<ChapterResponseDTO> GetChapterById(Guid id);
        Task<ChapterResponseDTO> CreateChapter(ChapterRequestDTO requestDTO);
        Task<ChapterResponseDTO> UpdateChapter(Guid id, UpdateChapterRequestDTO requestDTO);
        Task<bool> DeleteChapter(Guid id);

        Task<PaginatedResponseDTO<ChapterResponseDTO>> GetChapterBySubjectName(PaginationRequestDTO requestDTO, string subjectName);

    }
}
