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

            OrderEntity? entity = await Context.Orders
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity != null)
            {
                Context.Orders.Remove(entity);
                int affectedRows = await Context.SaveChangesAsync(cancellationToken);
                result = affectedRows > 0;
            }

            return result;
        }

        public async Task<OrderEntity?> Store(OrderModel model, CancellationToken cancellationToken)
        {
            OrderEntity? result = null;

            OrderEntity? currentEntity = await Context.Orders
                .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

            bool isNewEntity = currentEntity == null;

            if (isNewEntity)
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
                MapToExistingEntity(currentEntity, model);
                await Context.SaveChangesAsync(cancellationToken);
                result = currentEntity;
            }

            return result;
        }

        private static void MapToExistingEntity(OrderEntity entity, OrderModel model)
        {
            entity.CustomerId = model.CustomerId;
            entity.Total = model.Total;
            entity.Status = model.Status;
            entity.CompletedDate = model.CompletedDate;
            entity.CancelledDate = model.CancelledDate;
        }
    }
}