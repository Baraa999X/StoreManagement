using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.Orders
{
    public class GetOrderById
    {
        private readonly IOrderData _orderData;
        public GetOrderById(IOrderData orderData)
        {
            _orderData = orderData;
        }
        public async Task<OrderDto?> ExecuteAsync(int id)
        {
            var order = await _orderData.GetOrderByIdAsync(id);

            if (order == null)
            {
                return null;
            }

            var result = new OrderDto
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
            };

            return result;
        }

    }
}