using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }


        [EndpointDescription("API này lấy thông tin gói lên để đăng kí")]
        // GET: api/<SubscriptionController>
        [HttpGet]
        public async Task<IActionResult> GetAllUser(Guid subscriptionId)
        {
            var result = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId);
            return Ok(ResponseDTO<SubscriptionResponseDTO?>.Success(result, "Tìm thấy thông tin gói"));
        }
    }
}
