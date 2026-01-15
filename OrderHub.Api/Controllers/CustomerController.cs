using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHub.Core.Customer.Commands;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Specifications;

namespace OrderHub.Api.Controllers
{
    [Route("api/v{version:apiVersion}/customers")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator Mediator;

        public CustomerController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand.Request request)
        {
            IActionResult result = BadRequest();

            CustomerModel? customer = await Mediator.Send(request);

            if (customer != null)
            {
                result = CreatedAtAction(
                    nameof(GetCustomer),
                    new
                    {
                        version = "1.0",
                        id = customer.Id
                    },
                    customer);
            }

            return result;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers()
        {
            List<CustomerModel> result = await Mediator.Send(new SearchCustomersSpecification());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            IActionResult result = NotFound();

            SearchCustomerByIdSpecification specification = new()
            {
                Id = id
            };

            CustomerModel? customer = await Mediator.Send(specification);

            if (customer != null)
            {
                result = Ok(customer);
            }

            return result;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] UpdateCustomerCommand.Request request)
        {
            IActionResult result = BadRequest();

            request.Id = id;

            CustomerModel? customer = await Mediator.Send(request);

            if (customer != null)
            {
                result = Ok(customer);
            }

            return result;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            IActionResult result = NotFound();

            DeleteCustomerCommand.Request request = new()
            {
                Id = id
            };

            bool deleted = await Mediator.Send(request);

            if (deleted)
            {
                result = NoContent();
            }

            return result;
        }
    }
}