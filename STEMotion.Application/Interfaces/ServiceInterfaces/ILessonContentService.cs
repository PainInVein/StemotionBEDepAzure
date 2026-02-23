using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ILessonContentService
    {
        Task<IEnumerable<LessonContentResponseDTO>> GetContentsByLessonId(Guid lessonId);
        Task<LessonContentResponseDTO> GetById(Guid id);
        Task<LessonContentResponseDTO> Create(CreateLessonContentRequestDTO request);
        Task<LessonContentResponseDTO> Update(Guid id, UpdateLessonContentRequestDTO request);
        Task<bool> Delete(Guid id);
        Task<bool> ChangeStatus(Guid id, string newStatus);
        Task<IEnumerable<LessonContentResponseDTO>> GetContentsByLessonName(string lessonName);
    }
}
