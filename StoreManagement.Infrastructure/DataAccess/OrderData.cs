using Microsoft.EntityFrameworkCore;
using StoreManagement.Application.Interfaces;
using StoreManagement.Domain;
using StoreManagement.Domain.Entities;
using StoreManagement.Domain.Enums;
using StoreManagement.Infrastructure.Data;

namespace StoreManagement.Infrastructure
{
    public class OrderData : IOrderData
    {
        private readonly StoreDbContext _context;

        public OrderData(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }
        public async Task<Inventory?> GetInventoryByProductIdAsync(int productId)
        {
           return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);//فتش ضمن الانفنتوري عن اول انفنتوري يكون البرودكات اي دي يلي فيه يكون مساوي للبرودكت اي دي يلي عطيتك يا
        }
        public void UpdateInventoryAfterOrder(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
        }
        public async Task<List<Order>> GetAllOrdersAsync()//for Admin only
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByApplicationUserIdAsync(string applicationUserId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Customer.ApplicationUserId == applicationUserId)
                .ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> TryDecreaseInventoryIfAvailableAsync(int productId, int quantity)
        {
            var affectedRows = await _context.Inventories
                .Where(i => i.ProductId == productId && i.Quantity >= quantity)
                .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.Quantity, i => i.Quantity - quantity));
            return affectedRows > 0;
        }
        public async Task<bool> TryCreateOrderAsync(Order order,Dictionary<int, int> requestedQuantities)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in requestedQuantities)
                {
                    var affectedRows = await _context.Inventories
                        .Where(i => i.ProductId == item.Key &&i.Quantity >= item.Value) //حدّث سجل المخزون للمنتج المطلوب فقط إذا كانت الكمية المتاحة أكبر من أو تساوي الكمية المطلوبة
                        .ExecuteUpdateAsync(setters => setters.SetProperty( i => i.Quantity,i => i.Quantity - item.Value));//هاد بيخلّي SQL Server ينفّذ خصم الكمية مباشرة بقاعدة البيانات، بدل ما نعتمد فقط على قيمة المخزون اللي قرأناها سابقاً.
                    if (affectedRows == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> TryDeleteOrderAndRestoreInventoryAsync(int orderId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                if (order.Status != EnumsOrderStatus.Pending)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                foreach (var item in order.OrderItems)
                {
                    var affectedRows = await _context.Inventories
                        .Where(i => i.ProductId == item.ProductId)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(i => i.Quantity, i => i.Quantity + item.Quantity));

                    if (affectedRows == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> TryCancelOrderAndRestoreInventoryAsync(int orderId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await _context.Orders
                    .Where(o => o.Id == orderId &&
                        (o.Status == EnumsOrderStatus.Pending ||
                         o.Status == EnumsOrderStatus.Confirmed))
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(
                            o => o.Status,
                            EnumsOrderStatus.Cancelled));

                if (affectedRows == 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == orderId)
                    .ToListAsync();

                foreach (var item in orderItems)
                {
                    var inventoryUpdated = await _context.Inventories
                        .Where(i => i.ProductId == item.ProductId)
                        .ExecuteUpdateAsync(setters =>
                            setters.SetProperty(
                                i => i.Quantity,
                                i => i.Quantity + item.Quantity));

                    if (inventoryUpdated == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<Customer?> GetCustomerByApplicationUserIdAsync(string applicationUserId)//applicationUserId ب الاساسيه  من هون حقن عرف الشرط وطابق ال 
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == applicationUserId);
        }
        public async Task<bool> TryCancelMyOrderAndRestoreInventoryAsync(int orderId, string applicationUserId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var affectedRows = await _context.Orders
                    .Where(o => o.Id == orderId
                        && o.Customer.ApplicationUserId == applicationUserId
                        && (o.Status == EnumsOrderStatus.Pending|| o.Status == EnumsOrderStatus.Confirmed))
                    .ExecuteUpdateAsync(setters =>setters.SetProperty( o => o.Status,EnumsOrderStatus.Cancelled));

                if (affectedRows == 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == orderId)
                    .ToListAsync();

                foreach (var item in orderItems)
                {
                    var inventoryUpdated = await _context.Inventories
                        .Where(i => i.ProductId == item.ProductId)
                        .ExecuteUpdateAsync(setters =>
                            setters.SetProperty(
                                i => i.Quantity,
                                i => i.Quantity + item.Quantity));//خُد الكمية الموجودة حاليًا بالمخزون، وزيد عليها الكمية اللي كانت ضمن الطلب

                    if (inventoryUpdated == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}