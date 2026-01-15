using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Commands;
using OrderHub.Customer.Models;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerModel model)
        {
            IActionResult result = BadRequest();

            CustomerModel? customer = await Mediator.Send(new CreateCustomerCommand.Request()
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            });

            if(result != null)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers()
        {
            List<CustomerModel> result = await Mediator.Send(new SearchCustomersSpecification());

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}
