using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Application.DTOs
{
    public class CreateInventoryDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }
    }
}