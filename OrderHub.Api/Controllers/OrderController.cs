using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Queries;
using OrderHub.Core.Order.Specifications;
using OrderHub.Customer.Models;

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
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand.Request request)
        {
            IActionResult result = BadRequest();

            OrderModel? order = await Mediator.Send(request);

            if(result != null)
            {
                result = CreatedAtAction(
                   nameof(CreateOrder),
                   new
                   {
                       version = "1.0",
                       id = order.Id
                   },
                   order);
            }

            return result;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrders()
        {
            List<OrderModel> result = await Mediator.Send(new SearchOrdersSpecification());

            return Ok(result);
        }
    }
}
