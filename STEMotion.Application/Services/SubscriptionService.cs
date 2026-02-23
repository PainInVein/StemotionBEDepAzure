using AutoMapper;
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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubscriptionResponseDTO?> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            var result = await _unitOfWork.SubscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
            if (result == null)
            {
                throw new NotFoundException("Gói này không tồn tại");
            }
            var response = _mapper.Map<SubscriptionResponseDTO>(result);
            return response;
        }
    }
}
