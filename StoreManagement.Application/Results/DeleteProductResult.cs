using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Results
{
    public enum DeleteProductStatus
    {
        Success,
        NotFound,
        HasOrders
    }

    public class DeleteProductResult
    {
        public DeleteProductStatus Status { get; set; }

        public Product? Product { get; set; }
    }
}