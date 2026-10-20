using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Customers
{
    public class GetCustomerById
    {
        private readonly ICustomerData _customerData;

        public GetCustomerById(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public CustomerDto? Execute(int id)
        {
            var customer = _customerData.GetCustomerById(id);

            if (customer == null)
            {
                return null;
            }

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return customerDto;
        }
    }
}