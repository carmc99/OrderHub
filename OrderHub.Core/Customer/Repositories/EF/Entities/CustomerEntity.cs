using OrderHub.Customer.Models;

namespace OrderHub.Customer.Repositories.EF.Entities
{
    public class CustomerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public static CustomerEntity FromModel(CustomerModel model)
        {
            CustomerEntity entity = new();

            if (model != null)
            {
                entity = new()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address
                };
            }
            return entity;
        }
    }
}
