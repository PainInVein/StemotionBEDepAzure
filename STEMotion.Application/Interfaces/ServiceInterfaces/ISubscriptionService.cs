using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionResponseDTO?> GetSubscriptionByIdAsync(Guid subscriptionId);
    }
}
