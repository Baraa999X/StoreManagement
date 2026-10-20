using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Application.DTOs
{
    public class CreateOrderItemDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}