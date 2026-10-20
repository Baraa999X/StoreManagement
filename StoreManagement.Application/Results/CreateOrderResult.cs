namespace StoreManagement.Application.Results
{
    public enum CreateOrderResult
    {
        Success,
        CustomerNotFound,
        ProductNotFound,
        InventoryNotFound,
        InsufficientStock,
        InvalidQuantity,
        EmptyOrder
    }
}
