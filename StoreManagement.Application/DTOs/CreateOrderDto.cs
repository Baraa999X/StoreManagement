
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Application.DTOs
{
    public class CreateOrderDto
    {
        public int? CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
