
using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.Orders
{
    public class GetMyOrders
    {
        private readonly IOrderData _orderData;

        public GetMyOrders(IOrderData orderData)
        {
            _orderData = orderData;
        }

        public async Task<List<OrderDto>> ExecuteAsync(string applicationUserId)
        {
            var orders = await _orderData.GetOrdersByApplicationUserIdAsync(applicationUserId);

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,

                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
