using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ISubjectService 
    {
        Task<PaginatedResponseDTO<SubjectResponseDTO>> GetAllSubject(PaginationRequestDTO requestDTO);
        Task<SubjectResponseDTO> GetSubjectById(Guid id);
        Task<SubjectResponseDTO> CreateSubject(SubjectRequestDTO requestDTO);
        Task<SubjectResponseDTO> UpdateSubject(Guid id, UpdateSubjectRequestDTO requestDTO);
        Task<bool> DeleteSubject(Guid id);
        Task<PaginatedResponseDTO<SubjectResponseDTO>> GetSubjectByGradeLevel(PaginationRequestDTO requestDTO,int gradeLevel);
    }
}
