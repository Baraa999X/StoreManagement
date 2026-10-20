using StoreManagement.Domain;
using StoreManagement.Domain.Entities;
namespace StoreManagement.Application.Interfaces
{
    public interface IOrderData
    {
        Task<Customer?> GetCustomerByIdAsync(int customerId);

        Task<Product?> GetProductByIdAsync(int productId);

        Task AddOrderAsync(Order order);

        Task<Inventory?> GetInventoryByProductIdAsync(int productId);

        void UpdateInventoryAfterOrder(Inventory inventory);

        Task<List<Order>> GetAllOrdersAsync();// List<Order>ليش   //لان الغيت اول اوردر بده يرجع كل الطلبات مش اوردر واحد

        Task<List<Order>> GetOrdersByApplicationUserIdAsync(string applicationUserId);// مشان ياخذ طلبات بحسب اليوزر اي دي مو المستخدم هو يلي يدخل اي شي اوي يخبص بشي

        Task<Order?> GetOrderByIdAsync(int id);

        Task<Customer?> GetCustomerByApplicationUserIdAsync(string applicationUserId);

        Task UpdateOrderAsync(Order order);

        Task DeleteOrderAsync(Order order);//بنجيب الاوردر من id 

        Task<bool> TryDecreaseInventoryIfAvailableAsync(int productId, int quantity);// خصم الكمية من المخزون مباشرةً بقاعدة البيانات، بشرط أن تكون الكمية المتوفرة كافية

        Task<bool> TryCreateOrderAsync(Order order, Dictionary<int, int> requestedQuantities); // إنشاء الطلب وخصم كميات المخزون ضمن Transaction واحدة، لضمان نجاح العمليتين مع بعض

        Task<bool> TryDeleteOrderAndRestoreInventoryAsync(int orderId); // حذف الطلب واسترجاع كميات المخزون ضمن Transaction واحدة، لضمان نجاح العمليتين مع بعض

        Task<bool> TryCancelOrderAndRestoreInventoryAsync(int orderId); //بتغير حاله الطلب الى كانسليد وتبرجع القيمه للمخزون وبتحفظه تمن السجل ادمن لمفروض هي

        Task<bool> TryCancelMyOrderAndRestoreInventoryAsync(int orderId,string applicationUserId);// للعميل
    }
}