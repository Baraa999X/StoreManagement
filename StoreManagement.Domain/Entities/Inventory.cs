namespace StoreManagement.Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public int MinimumStock { get; set; }

        public DateTime LastRestocked { get; set; }
    }
}