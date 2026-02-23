using Microsoft.AspNetCore.Mvc;
using PayOS.Models.Webhooks;
using PayOS;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;
using System.Threading.Tasks;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly ISubscriptionPaymentService _subscriptionPaymentService;

        public SubscriptionPaymentController(ISubscriptionPaymentService subscriptionPaymentService)
        {
            _subscriptionPaymentService = subscriptionPaymentService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] Webhook webhookData)
        {
            var result = await _subscriptionPaymentService.UpdateSuccessPaymentByOrderCode(webhookData);

            if (string.IsNullOrEmpty(result))
            {
                return Ok(ResponseDTO<bool>.Success(true, "Cập nhật order thành công"));
            }
            else
            {
                return BadRequest(ResponseDTO<string>.Fail("Cập nhật order thất bại", result));
            }
        }

        [HttpGet("cancel-payment")]
        public async Task<IActionResult> HandlePaymentCancel([FromQuery] PaymentCancelRequestDTO paymentCancelRequestDTO)
        {
            if (paymentCancelRequestDTO.OrderCode <= 0)
            {
                return BadRequest(ResponseDTO<string>.Fail("Fail", "Thiếu hoặc orderCode không hợp lệ"));
            }

            var result = await _subscriptionPaymentService.UpdateCancelPaymentByOrderCode(paymentCancelRequestDTO);

            if (string.IsNullOrEmpty(result))
            {
                return Ok(ResponseDTO<bool>.Success(true, "Hủy payment thành công"));
            }
            else
            {
                return BadRequest(ResponseDTO<string>.Fail("Hủy payment thất bại", result));
            }
        }
    }
}
