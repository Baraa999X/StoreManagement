using StoreManagement.Domain.Enums;

namespace StoreManagement.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;// تمثل علاقه الون من جهه

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // تمثل علاقه المني من جهه

        public DateTime OrderDate { get; set; }

        public EnumsOrderStatus Status { get; set; }
    }
}