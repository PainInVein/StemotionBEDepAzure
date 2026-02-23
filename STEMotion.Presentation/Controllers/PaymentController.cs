using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using STEMotion.Application.DTO.RequestDTOs.PaymentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        //private readonly PayOSClient _payOSClient;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [EndpointDescription("API này check user mua gói chưa")]
        // GET: api/<PaymentController>
        [HttpGet]
        public async Task<IActionResult> GetAllUser(Guid userId)
        {
            var result = await _paymentService.CheckUserPaymentAsync(userId);
            return Ok(ResponseDTO<bool>.Success(result, "User đã mua gói"));
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> Create([FromBody] PaymentInfoRequestDTO paymentInfoRequestDTO)
        {
            var response = await _paymentService.CreatePaymentLinkResponse(paymentInfoRequestDTO);

            return Ok(ResponseDTO<PaymentResponseDTO>.Success(response, "Tạo link thanh toán thành công"));
        }

        //[HttpPost("webhook")]
        //public async Task<IActionResult> HandleWebhook([FromBody] Webhook webhookData)
        //{
        //    try
        //    {
        //        var verifiedData = await _payOSClient.Webhooks.VerifyAsync(webhookData);
        //        Console.WriteLine($"Thanh toán thành công: {verifiedData.OrderCode}");
        //        return Ok("OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Webhook không hợp lệ: {ex.Message}");
        //        return BadRequest("Invalid webhook");
        //    }
        //}

        //[HttpPost("payment/{orderCode}/cancel")]
        //public async Task<IActionResult> Cancel(long orderCode)
        //{
        //    var result = await _payOSClient.PaymentRequests.CancelAsync(orderCode);
        //    return Ok(result);
        //}

    }
}
