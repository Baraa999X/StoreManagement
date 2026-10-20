using StoreManagement.Domain.Enums;

namespace StoreManagement.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumsOrderStatus Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
//نفس الشي بيانات خارجة