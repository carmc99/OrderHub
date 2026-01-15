using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Core.Order.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }

        public static OrderModel FromEntity(OrderEntity entity)
        {
            OrderModel order = new();

            if (entity != null)
            {
                order = new()
                {
                    Id = entity.Id,
                    CustomerId = entity.CustomerId,
                    OrderDate = entity.OrderDate,
                    Total = entity.Total,
                    Status = entity.Status,
                    CompletedDate = entity.CompletedDate,
                    CancelledDate = entity.CancelledDate
                };
            }

            return order;
        }
    }
}
