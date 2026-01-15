using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Core.Order.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> Delete(int id, CancellationToken cancellationToken);

        Task<OrderEntity?> Store(OrderModel model, CancellationToken cancellationToken);
    }
}
