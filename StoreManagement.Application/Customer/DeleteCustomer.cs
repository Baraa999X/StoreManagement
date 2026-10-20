using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Customers
{
    public class DeleteCustomer
    {
        private readonly ICustomerData _customerData;

        public DeleteCustomer(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public async Task<CustomerDto?> ExecuteAsync(int id)
        {
            var deletedCustomer =
                await _customerData.DeleteCustomerAsync(id);

            if (deletedCustomer == null)
            {
                return null;
            }

            var customerDto = new CustomerDto
            {
                Id = deletedCustomer.Id,
                Name = deletedCustomer.Name,
                Email = deletedCustomer.Email,
                Phone = deletedCustomer.Phone
            };

            return customerDto;
        }
    }
}