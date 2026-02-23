using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;

namespace STEMotion.Application.Services
{
    public class LessonContentService : ILessonContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonContentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LessonContentResponseDTO>> GetContentsByLessonName(string lessonName)
        {
            var lesson = await _unitOfWork.LessonRepository
                .FindByCondition(l => l.LessonName.ToLower() == lessonName.ToLower() && l.Status == "Active")
                .FirstOrDefaultAsync();

            if (lesson == null)
                throw new NotFoundException("Lesson", lessonName);

            return await GetContentsByLessonId(lesson.LessonId);
        }
        public async Task<IEnumerable<LessonContentResponseDTO>> GetContentsByLessonId(Guid lessonId)
        {
            var contents = await _unitOfWork.LessonContentRepository
                .FindByCondition(c => c.LessonId == lessonId && c.Status == "Active")
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonContentResponseDTO>>(contents);
        }

        public async Task<LessonContentResponseDTO> GetById(Guid id)
        {
            var content = await _unitOfWork.LessonContentRepository.GetByIdAsync(id);
            return _mapper.Map<LessonContentResponseDTO>(content);
        }

        public async Task<LessonContentResponseDTO> Create(CreateLessonContentRequestDTO request)
        {
            var lesson = await _unitOfWork.LessonRepository
                .FindByCondition(l =>
                    l.LessonId == request.LessonId &&
                    l.Status == "Active")
                .FirstOrDefaultAsync();

            if (lesson == null)
                throw new NotFoundException($"Không tìm thấy bài học.");

            var content = _mapper.Map<LessonContent>(request);
            content.LessonId = lesson.LessonId;
            content.Status = "Active";

            await _unitOfWork.LessonContentRepository.CreateAsync(content);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonContentResponseDTO>(content);
        }

        public async Task<LessonContentResponseDTO> Update(Guid id, UpdateLessonContentRequestDTO request)
        {
            var existingContent = await _unitOfWork.LessonContentRepository.GetByIdAsync(id);

            if (existingContent == null)
                throw new NotFoundException("LessonContent", id);

            _mapper.Map(request, existingContent);

            _unitOfWork.LessonContentRepository.Update(existingContent);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonContentResponseDTO>(existingContent);
        }

        public async Task<bool> Delete(Guid id)
        {
            var content = await _unitOfWork.LessonContentRepository.GetByIdAsync(id);
            if (content == null) return false;

            _unitOfWork.LessonContentRepository.Delete(content);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> ChangeStatus(Guid id, string newStatus)
        {
            var content = await _unitOfWork.LessonContentRepository.GetByIdAsync(id);
            if (content == null) return false;

            content.Status = newStatus;
            _unitOfWork.LessonContentRepository.Update(content);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}