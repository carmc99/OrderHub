using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public static CustomerModel FromEntity(CustomerEntity entity)
        {
            CustomerModel customer = new();

            if (entity != null)
            {
                customer = new()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    Address = entity.Address
                };
            }

            return customer;
        }

    }
}
