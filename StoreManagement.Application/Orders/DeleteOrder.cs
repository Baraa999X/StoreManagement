using StoreManagement.Application.Interfaces;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Orders
{
    public class DeleteOrder
    {
        private readonly IOrderData _orderData;

        public DeleteOrder(IOrderData orderData)
        {
            _orderData = orderData;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            // 1. Delete the order and restore inventory quantities
            return await _orderData.TryDeleteOrderAndRestoreInventoryAsync(id);
        }
    }
}