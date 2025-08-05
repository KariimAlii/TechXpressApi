using BusinessLayer.Managers.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Api.ActionRequests;

namespace PresentationLayer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateOrderActionRequest request)
        {
            var updateOrderDto = request.ToDto();
            updateOrderDto.Id = id;
            await _orderManager.UpdateOrder(updateOrderDto);
            return Ok();
        }
    }
}
