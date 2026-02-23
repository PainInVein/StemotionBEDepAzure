using PayOS.Models.Webhooks;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface ISubscriptionPaymentService
    {
        Task<string> UpdateSuccessPaymentByOrderCode(Webhook webhook);

        Task<string> UpdateCancelPaymentByOrderCode(PaymentCancelRequestDTO paymentCancelRequestDTO);
    }
}
