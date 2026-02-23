using PayOS.Models.V2.PaymentRequests;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IPaymentService
    {
        Task<bool> CheckUserPaymentAsync(Guid userId);

        Task<PaymentResponseDTO> CreatePaymentLinkResponse(PaymentInfoRequestDTO paymentInfoRequestDTO);
    }
}
