using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Contracts
{
    public interface ICustomerData
    {
        ICollection<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        Customer AddCustomer(Customer customer);
        Customer? UpdateCustomer(int id, Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);
        Customer AddCustomerForUser(Customer customer, string applicationUserId);

    }
}