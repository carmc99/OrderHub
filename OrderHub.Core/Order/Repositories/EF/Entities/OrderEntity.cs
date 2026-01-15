using OrderHub.Core.Order.Models;

namespace OrderHub.Core.Order.Repositories.EF.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }

        public static OrderEntity FromModel(OrderModel model)
        {
            OrderEntity entity = new();

            if (model != null)
            {
                entity = new()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    OrderDate = model.OrderDate,
                    Total = model.Total,
                    Status = model.Status,
                    CompletedDate = model.CompletedDate,
                    CancelledDate = model.CancelledDate
                };
            }

            return entity;
        }
    }
}
