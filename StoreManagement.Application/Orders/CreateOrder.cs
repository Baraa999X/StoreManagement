using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;
using StoreManagement.Application.Results;
using StoreManagement.Domain.Entities;
using StoreManagement.Domain.Enums;

namespace StoreManagement.Application.Orders
{
    public class CreateOrder
    {
        private readonly IOrderData _orderData;
        public CreateOrder(IOrderData orderData)
        {
            _orderData = orderData;
        }

        public async Task<CreateOrderResult> ExecuteAsync(CreateOrderDto dto, string? userId, bool isAdmin)
        {
            // 1. Check Customer
            Customer? customer;

            if (isAdmin)
            {
                if (dto.CustomerId == null || dto.CustomerId <= 0)
                {
                    return CreateOrderResult.CustomerNotFound;
                }

                customer = await _orderData.GetCustomerByIdAsync(dto.CustomerId.Value);
            }
            else
            {
                // Customer must use their own account
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return CreateOrderResult.CustomerNotFound;
                }

                customer = await _orderData.GetCustomerByApplicationUserIdAsync(userId);
            }

            if (customer == null)
            {
                return CreateOrderResult.CustomerNotFound;
            }

            if (dto.Items == null || dto.Items.Count == 0)
            {
                return CreateOrderResult.EmptyOrder;
            }
            // 2. Check Products and Inventory
            var groupedItems = dto.Items//قائمة المنتجات اللي طلبها المستخدم
                .GroupBy(item => item.ProductId) // بيجمع العناصر اللي إلها نفس رقم المنتج
                .Select(group => new
                {
                    ProductId = group.Key,// رقم المنتج ضمن المجموعةهو يلي بعمله غروب المفتاح مشان قارن لاحقا
                    TotalQuantity = group.Sum(item => item.Quantity)//بتجمع قيمة Quantity لكل العناصر الموجودة داخل نفس المجموعة.س
                })
                .ToList();

            foreach (var itemDto in dto.Items)
            {
                // Check Product
                var product = await _orderData.GetProductByIdAsync(itemDto.ProductId);

                if (product == null)
                {
                    return CreateOrderResult.ProductNotFound;
                }

                // Check Inventory
                var inventory = await _orderData.GetInventoryByProductIdAsync(itemDto.ProductId);

                if (inventory == null)
                {
                    return CreateOrderResult.InventoryNotFound;
                }

                // Check Quantity
                if (itemDto.Quantity <= 0)
                {
                    return CreateOrderResult.InvalidQuantity;
                }

                // Check Total Quantity for Duplicate Products
                var groupedItem = groupedItems//جبنا إجمالي كمية المنتج
                    .First(group => group.ProductId == itemDto.ProductId);//روح على groupedItems، وابحث عن أول مجموعة رقم منتجها بيساوي رقم المنتج الموجود في itemDto، ورجّعلي هاي المجموعة

                if (groupedItem.TotalQuantity > inventory.Quantity)//قارنّا الإجمالي بالمخزون
                {
                    return CreateOrderResult.InsufficientStock;
                }
            }

            // 3. Create Order
            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                Status = EnumsOrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            // 4. Create OrderItems
            foreach (var itemDto in dto.Items)
            {
                var product = await _orderData.GetProductByIdAsync(itemDto.ProductId);

                var orderItem = new OrderItem
                {
                    ProductId = product!.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
            }

            // 5. جمع إجمالي الكمية المطلوبة من كل منتج
            var requestedQuantities = groupedItems.ToDictionary(item => item.ProductId, item => item.TotalQuantity);
            // 6. خصم المخزون وحفظ الطلب ضمن Transaction واحدة
            var orderCreated = await _orderData.TryCreateOrderAsync(order,requestedQuantities);
            if (!orderCreated)
            {
                return CreateOrderResult.InsufficientStock;
            }

            return CreateOrderResult.Success;
        }
    }
}
