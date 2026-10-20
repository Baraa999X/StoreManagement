using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;
using StoreManagement.Domain.Enums;

namespace StoreManagement.Application.Orders
{
    public class UpdateOrderStatus
    {
        private readonly IOrderData _orderData;

        public UpdateOrderStatus(IOrderData orderData)
        {
            _orderData = orderData;
        }
        public async Task<bool> ExecuteAsync(int id, UpdateOrderStatusDto dto)
        {
            if (!Enum.IsDefined(typeof(EnumsOrderStatus), dto.Status))
            {
                return false;
            }

            if (dto.Status == EnumsOrderStatus.Cancelled)
            {
                return await _orderData.TryCancelOrderAndRestoreInventoryAsync(id);
            }

            var order = await _orderData.GetOrderByIdAsync(id);

            if (order == null || order.Status == EnumsOrderStatus.Cancelled)
            {
                return false;
            }
            var isValidTransition = order.Status switch
            {
                EnumsOrderStatus.Pending =>
                    dto.Status == EnumsOrderStatus.Confirmed,

                EnumsOrderStatus.Confirmed =>
                    dto.Status == EnumsOrderStatus.Shipped,

                EnumsOrderStatus.Shipped =>
                    dto.Status == EnumsOrderStatus.Delivered,

                _ => false
            };

            if (!isValidTransition)
            {
                return false;
            }
            order.Status = dto.Status;
            await _orderData.UpdateOrderAsync(order);

            return true;
        }
    }
}