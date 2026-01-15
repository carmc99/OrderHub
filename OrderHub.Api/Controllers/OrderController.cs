using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Specifications;

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

            if (order != null)
            {
                result = CreatedAtAction(
                   nameof(GetOrder),
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
        [ProducesResponseType(typeof(List<OrderModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrders()
        {
            List<OrderModel> result = await Mediator.Send(new SearchOrdersSpecification());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            IActionResult result = NotFound();

            SearchOrderByIdSpecification specification = new()
            {
                Id = id
            };

            OrderModel? order = await Mediator.Send(specification);

            if (order != null)
            {
                result = Ok(order);
            }

            return result;
        }

        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CompleteOrder([FromRoute] int id)
        {
            IActionResult result = NotFound();

            CompleteOrderCommand.Request request = new()
            {
                Id = id
            };

            OrderModel? order = await Mediator.Send(request);

            if (order != null)
            {
                result = Ok(order);
            }

            return result;
        }

        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(typeof(OrderModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            IActionResult result = NotFound();

            CancelOrderCommand.Request request = new()
            {
                Id = id
            };

            OrderModel? order = await Mediator.Send(request);

            if (order != null)
            {
                result = Ok(order);
            }

            return result;
        }
    }
}