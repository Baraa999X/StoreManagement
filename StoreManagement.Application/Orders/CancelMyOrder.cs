using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.Orders
{
    public class CancelMyOrder
    {
        private readonly IOrderData _orderData;

        public CancelMyOrder(IOrderData orderData)
        {
            _orderData = orderData;
        }
        public async Task<bool> ExecuteAsync(int orderId, string applicationUserId)
        {
            return await _orderData.TryCancelMyOrderAndRestoreInventoryAsync(orderId,applicationUserId);

        }
    }
}