using AutoMapper;
using PayOS;
using PayOS.Models.Webhooks;
using PayOS.Resources.Webhooks;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;
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
    public class SubscriptionPaymentService : ISubscriptionPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSClient _payOSClient;

        public SubscriptionPaymentService(IUnitOfWork unitOfWork, IMapper mapper, PayOSClient payOSClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSClient = payOSClient;
        }

        public async Task<string> UpdateSuccessPaymentByOrderCode(Webhook webhook)
        {

            try
            {
                var verifiedData = await _payOSClient.Webhooks.VerifyAsync(webhook);
            }
            catch (Exception ex)
            {
                return $"Webhook không hợp lệ: {ex.Message}";
            }

            var subscriptionPaymentByOrderCode = await _unitOfWork.SubscriptionPaymentRepository.GetSubscriptionPaymentByOrderCodeAsync(webhook.Data.OrderCode);

            if (subscriptionPaymentByOrderCode == null)
            {
                return "Không tìm thấy OrderCode";
            }
            _mapper.Map(webhook.Data, subscriptionPaymentByOrderCode);

            subscriptionPaymentByOrderCode.Success = true;
            subscriptionPaymentByOrderCode.Payment.Status = "Paid";
            subscriptionPaymentByOrderCode.Description = webhook.Data.Description;

            _unitOfWork.SubscriptionPaymentRepository.Update(subscriptionPaymentByOrderCode);
            await _unitOfWork.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string> UpdateCancelPaymentByOrderCode(PaymentCancelRequestDTO paymentCancelRequestDTO)
        {
            var subscriptionPaymentByOrderCode = await _unitOfWork.SubscriptionPaymentRepository.GetSubscriptionPaymentByOrderCodeAsync(paymentCancelRequestDTO.OrderCode);

            if (subscriptionPaymentByOrderCode == null)
            {
                return "Không tìm thấy OrderCode";
            }

            subscriptionPaymentByOrderCode.Success = false;
            subscriptionPaymentByOrderCode.Payment.Status = "Cancelled";
            subscriptionPaymentByOrderCode.Description = "Payment cancelled by user.";
            subscriptionPaymentByOrderCode.PaymentLinkId = paymentCancelRequestDTO.PaymentLinkId;
            subscriptionPaymentByOrderCode.Code = paymentCancelRequestDTO.Code;
            _unitOfWork.SubscriptionPaymentRepository.Update(subscriptionPaymentByOrderCode);
            await _unitOfWork.SaveChangesAsync();

            return string.Empty;
        }
    }
}
