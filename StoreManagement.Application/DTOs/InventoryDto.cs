namespace StoreManagement.Application.DTOs
{
    public class InventoryDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int MinimumStock { get; set; }

        public DateTime LastRestocked { get; set; }
    }
}