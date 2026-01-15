using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            CustomerModel? result = await Mediator.Send(new CreateCustomerCommand.Request()
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            });

            if(result != null)
            {
                //TODO: Get
                return CreatedAtAction(nameof(CreateCustomer), 
                    new 
                    {
                        id = result.Id
                    }, result);
            }

            return BadRequest();
        }
    }
}
