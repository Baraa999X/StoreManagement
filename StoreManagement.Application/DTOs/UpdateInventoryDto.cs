using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Application.DTOs
{
    public class UpdateInventoryDto
    {
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }
    }
}