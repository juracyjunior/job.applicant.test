using BestHB.Domain.Service;
using BestHB.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BestHB.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        public readonly IOrderService _orderService;

        public UsersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("{userId}/orders")]
        public async Task<IActionResult> Add([FromRoute] int userId)
        {
            var orders = await _orderService.GetListByUserAsync(userId);

            if (orders.HasError)
                return BadRequest(orders.Errors);

            return Ok(orders.Data);
        }
    }
}
