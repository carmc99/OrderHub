using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Core.Order.Repositories.EF
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext Context;

        public OrderRepository(OrderDbContext context)
        {
            Context = context;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = false;

            OrderEntity? entity = await Context.Set<OrderEntity>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity != null)
            {
                Context.Set<OrderEntity>().Remove(entity);
                int affectedRows = await Context.SaveChangesAsync(cancellationToken);
                result = affectedRows > 0;
            }

            return result;
        }

        public async Task<OrderEntity?> Store(OrderModel model, CancellationToken cancellationToken)
        {
            OrderEntity? result = null;

            if (model.Id == 0)
            {
                OrderEntity newEntity = OrderEntity.FromModel(model);
                newEntity.OrderDate = DateTime.UtcNow;
                newEntity.Status = OrderStatus.Pending;

                Context.Orders.Add(newEntity);
                await Context.SaveChangesAsync(cancellationToken);

                result = newEntity;
            }
            else
            {
                OrderEntity? existingEntity = await Context.Set<OrderEntity>()
                    .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

                if (existingEntity != null)
                {
                    existingEntity.CustomerId = model.CustomerId;
                    existingEntity.Total = model.Total;
                    existingEntity.Status = model.Status;
                    existingEntity.CompletedDate = model.CompletedDate;
                    existingEntity.CancelledDate = model.CancelledDate;

                    Context.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    await Context.SaveChangesAsync(cancellationToken);
                    result = existingEntity;
                }
            }

            return result;
        }
    }
}
