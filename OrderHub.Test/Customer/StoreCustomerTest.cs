using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using OrderHub.Customer.Commands;

namespace OrderHub.Test.Customer
{
    public class StoreCustomerTest
    {
        [Fact]
        public async Task GivenValidCustomer_WhenCreatingCustomer_ThenShouldReturnTrue()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsSuccess();
                });

            CreateCustomerCommand.Request request = new()
            {
                Name = "Juan Pérez",
                Email = "juan@example.com",
                PhoneNumber = "3001234567",
                Address = "Calle 10 #20-30"
            };

            // When
            bool result = await serviceProvider.WhenCreateCustomer(request);

            // Then
            result.ThenShouldCompleteSuccessfully();
        }
    }
}
