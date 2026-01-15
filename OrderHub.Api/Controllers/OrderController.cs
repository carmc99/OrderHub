using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;

namespace OrderHub.Api.Controllers
{
    [Route("api/v{version:apiVersion}/orders")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator Mediator;

        public OrderController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel model)
        {
            IActionResult result = BadRequest();

            OrderModel? order = await Mediator.Send(new CreateOrderCommand.Request()
            {
               Status = model.Status,
               CustomerId = model.CustomerId,
               Total = model.Total
            });

            if(result != null)
            {
                //TODO: 
            }

            return result;
        }
    }
}
