using AutoMapper;
using Microsoft.Extensions.Configuration;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;

namespace STEMotion.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSClient _payOSClient;
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, PayOSClient payOSClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSClient = payOSClient;
            _configuration = configuration;
        }

        public async Task<bool> CheckUserPaymentAsync(Guid userId)
        {
            return await _unitOfWork.PaymentRepository.CheckUserPaymentAsync(userId);
        }

        public async Task<PaymentResponseDTO> CreatePaymentLinkResponse(PaymentInfoRequestDTO paymentInfoRequestDTO)
        {
            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var paymentRequest = new CreatePaymentLinkRequest
            {
                OrderCode = orderCode,
                Amount = paymentInfoRequestDTO.SubscriptionInfo.SubscriptionPrice,
                Description = $"Gói {paymentInfoRequestDTO.SubscriptionInfo.BillingPeriod} - {paymentInfoRequestDTO.SubscriptionInfo.SubscriptionName}",
                CancelUrl = _configuration["PayOSLinks:CancelUrl"]!,
                ReturnUrl = _configuration["PayOSLinks:SuccessUrl"]!
            };

            //Create payment in database
            Payment pendingPayment = new Payment();
            pendingPayment.Info = $"Thanh toán gói {paymentInfoRequestDTO.SubscriptionInfo.SubscriptionName} - {paymentInfoRequestDTO.SubscriptionInfo.BillingPeriod}";
            pendingPayment.Status = "Pending";
            pendingPayment.UserId = paymentInfoRequestDTO.UserId;
            pendingPayment.Amount = paymentInfoRequestDTO.SubscriptionInfo.SubscriptionPrice;
            pendingPayment.PaymentDate = DateTime.UtcNow;

            pendingPayment.SubscriptionPayment = new SubscriptionPayment
            {
                OrderCode = orderCode,
                SubscriptionId = paymentInfoRequestDTO.SubscriptionInfo.SubscriptionId,
                Amount = paymentInfoRequestDTO.SubscriptionInfo.SubscriptionPrice
            };

            //tao payment
            var request = await _unitOfWork.PaymentRepository.CreateAsync(pendingPayment);

            if (request == null)
            {
                throw new InternalServerException("Không thể tạo payment");
            }

            await _unitOfWork.SaveChangesAsync();

            //create payment link
            var payOSResponse = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);

            var response = _mapper.Map<PaymentResponseDTO>(payOSResponse);

            return response;
        }
    }
}
