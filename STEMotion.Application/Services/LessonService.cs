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
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LessonResponseDTO> CreateLesson(LessonRequestDTO requestDTO)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetChapterWithSubjectAndGradeAsync(requestDTO.ChapterId);

            if (chapter == null)
            {
                throw new NotFoundException("Chương không tồn tại");
            }
            var isDuplicate = await _unitOfWork.LessonRepository.ExistsAsync(x =>
                x.LessonName == requestDTO.LessonName &&
                x.ChapterId == requestDTO.ChapterId
            );



            if (isDuplicate)
                throw new AlreadyExistsException("Bài học", requestDTO.LessonName);
            var lesson = _mapper.Map<Lesson>(requestDTO);
            lesson.Status = "Active";
            lesson.ChapterId = chapter.ChapterId;
            var request = await _unitOfWork.LessonRepository.CreateAsync(lesson);
            await _unitOfWork.SaveChangesAsync();
            if (request == null)
            {
                throw new InternalServerException("Không thể tạo được bài học");
            }
            var response = _mapper.Map<LessonResponseDTO>(request);
            response.ChapterName = chapter.ChapterName;
            response.GradeLevel = chapter.Subject.Grade.GradeLevel;
            return response;
        }

        public async Task<bool> DeleteLesson(Guid id)
        {
            var findingLesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (findingLesson == null)
            {
                throw new NotFoundException("Bài học không tồn tại");
            }
            _unitOfWork.LessonRepository.Delete(findingLesson);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponseDTO<LessonResponseDTO>> GetAllLesson(PaginationRequestDTO requestDTO)
        {
            var (lesson, total) = await _unitOfWork.LessonRepository.GetPagedAsync(requestDTO.PageNumber, requestDTO.PageSize, null, x => x.Chapter, x => x.Chapter.Subject, x => x.Chapter.Subject.Grade);
            var response = _mapper.Map<IEnumerable<LessonResponseDTO>>(lesson);
            return new PaginatedResponseDTO<LessonResponseDTO>
            {
                Items = response,
                PageSize = requestDTO.PageSize,
                PageNumber = requestDTO.PageNumber,
                TotalCount = total
            };
        }

        public async Task<LessonResponseDTO> GetLessonById(Guid id)
        {
            var result = await _unitOfWork.LessonRepository.FindByCondition(x => x.LessonId == id, false).Include(x => x.Chapter)
            .ThenInclude(c => c.Subject)
            .ThenInclude(s => s.Grade)
            .FirstOrDefaultAsync();
            if (result == null)
            {
                throw new NotFoundException("Bài học này không tồn tại");
            }
            var response = _mapper.Map<LessonResponseDTO>(result);
            return response;
        }

        public async Task<PaginatedResponseDTO<LessonResponseDTO>> GetSubjectByChapterName(PaginationRequestDTO requestDTO, string chapterName)
        {
            var (lesson, total) = await _unitOfWork.LessonRepository.GetPagedAsync(requestDTO.PageNumber, requestDTO.PageSize, x => x.Chapter.ChapterName == chapterName, x => x.Chapter, x => x.Chapter.Subject,
        x => x.Chapter.Subject.Grade);
            var response = _mapper.Map<IEnumerable<LessonResponseDTO>>(lesson);
            return new PaginatedResponseDTO<LessonResponseDTO>
            {
                Items = response,
                PageSize = requestDTO.PageSize,
                PageNumber = requestDTO.PageNumber,
                TotalCount = total
            };
        }

        public async Task<LessonResponseDTO> UpdateLesson(Guid id, UpdateLessonRequestDTO requestDTO)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                throw new NotFoundException("Bài học này không tồn tại");
            }
            _mapper.Map(requestDTO, lesson);
            _unitOfWork.LessonRepository.Update(lesson);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<LessonResponseDTO>(lesson);
            return response;
        }
    }
}
