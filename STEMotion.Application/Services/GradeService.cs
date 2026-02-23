using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #region cto
        public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion cto
        #region CRUD
        public async Task<GradeResponseDTO> CreateGrade(GradeRequestDTO requestDTO)
        {
            var existingGrade = await _unitOfWork.GradeRepository.ExistsAsync(x => x.GradeLevel == requestDTO.GradeLevel);
            if (existingGrade)
            {
                throw new AlreadyExistsException("Lớp", $"{requestDTO.GradeLevel}");
            }
            var grade = _mapper.Map<Grade>(requestDTO);
            grade.Status = "Active";
            var request = await _unitOfWork.GradeRepository.CreateAsync(grade);
            await _unitOfWork.SaveChangesAsync();
            if (request == null)
            {
                throw new InternalServerException("Không thể tạo lớp");
            }
            var response = _mapper.Map<GradeResponseDTO>(request);
            return response;
        }

        public async Task<bool> DeleteGrade(Guid id)
        {

            var findingGrade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
            if (findingGrade == null)
            {
                throw new NotFoundException("Lớp không tồn tại");
            }
            _unitOfWork.GradeRepository.Delete(findingGrade);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponseDTO<GradeResponseDTO>> GetAllGrade(PaginationRequestDTO requestDTO)
        {
            var (grade, total) = await _unitOfWork.GradeRepository.GetPagedAsync(requestDTO.PageNumber, requestDTO.PageSize);
            var response = _mapper.Map<IEnumerable<GradeResponseDTO>>(grade);
            return new PaginatedResponseDTO<GradeResponseDTO>
            {
                Items = response,
                PageNumber = requestDTO.PageNumber,
                PageSize = requestDTO.PageSize,
                TotalCount = total
            };
        }

        public async Task<GradeResponseDTO> GetGradeById(Guid id)
        {
            var result = await _unitOfWork.GradeRepository.GetByIdAsync(id);
            if (result == null)
            {
                throw new NotFoundException("Lớp không tồn tại");
            }
            var response = _mapper.Map<GradeResponseDTO>(result);
            return response;
        }

        public async Task<GradeResponseDTO> UpdateGrade(Guid id, UpdateGradeRequest requestDTO)
        {

            var grade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
            if (grade == null)
            {
                throw new NotFoundException("Lớp không tồn tại");
            }
            _mapper.Map(requestDTO, grade);
            _unitOfWork.GradeRepository.Update(grade);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<GradeResponseDTO>(grade);
            return response;
        }
        #endregion CRUD
    }
}
