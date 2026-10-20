using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.DTOs;
using StoreManagement.Application.Orders;
using StoreManagement.Application.Results;
using StoreManagement.Domain.Enums;
using System.Security.Claims;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrder _createOrder;
        private readonly GetAllOrders _getAllOrders;
        private readonly GetOrderById _getOrderById;
        private readonly UpdateOrderStatus _updateOrderStatus;
        private readonly DeleteOrder _deleteOrder;
        private readonly GetMyOrders _getMyOrders;
        private readonly CancelMyOrder _cancelMyOrder;
        public OrdersController(CreateOrder createOrder, GetAllOrders getAllOrders, GetOrderById getOrderById, UpdateOrderStatus updateOrderStatus, DeleteOrder deleteOrder, GetMyOrders getMyOrders, CancelMyOrder cancelMyOrder)
        {
            _createOrder = createOrder;
            _getAllOrders = getAllOrders;
            _getOrderById = getOrderById;
            _updateOrderStatus = updateOrderStatus;
            _deleteOrder = deleteOrder;
            _getMyOrders = getMyOrders;
            _cancelMyOrder = cancelMyOrder;
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)//هون ASP.NET بياخد الـ JSON اللي جاي من postman ويحوّله إلى:
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// هي داتا الاثوريشن  
            var isAdmin = User.IsInRole("Admin");
            var result = await _createOrder.ExecuteAsync(dto, userId, isAdmin);//بعات الداتا يلي وصلت
            return result switch
            {
                CreateOrderResult.Success => StatusCode(201, "Order created successfully."),
                CreateOrderResult.CustomerNotFound => NotFound("Customer not found."),
                CreateOrderResult.ProductNotFound => NotFound("Product not found."),
                CreateOrderResult.InventoryNotFound => BadRequest("Inventory not found for this product."),
                CreateOrderResult.InsufficientStock => BadRequest("Insufficient stock."),
                CreateOrderResult.InvalidQuantity => BadRequest("Quantity must be greater than zero."),
                CreateOrderResult.EmptyOrder => BadRequest("Order must contain at least one item."),
                _ => BadRequest("Unable to create order.")

            };
        }
        [Authorize(Roles = "Admin")]
        [HttpGet] public async Task<IActionResult> GetAll() 
        { var orders = await _getAllOrders.ExecuteAsync(); 
            return Ok(orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _getOrderById.ExecuteAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]//الساتس هي موجوده انت تبتعتها لما بتبعت الاي دي مثلا خمسه سلاش/api/Orders/5/status
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            if (!Enum.IsDefined(typeof(EnumsOrderStatus), dto.Status))
            {
                return BadRequest("Invalid order status.");
            }
            var result = await _updateOrderStatus.ExecuteAsync(id, dto);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deleteOrder.ExecuteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId==null)
            {
                return Unauthorized();
            }
            var orders = await _getMyOrders.ExecuteAsync(userId);
            return Ok(orders);
        }
        [Authorize(Roles = "Customer")]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelMyOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var cancel = await _cancelMyOrder.ExecuteAsync(id, userId);
            if (!cancel)
                return NotFound();
            return Ok();
        }

    }
}