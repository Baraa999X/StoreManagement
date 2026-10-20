using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Customers
{
    public class AddCustomer
    {
        private readonly ICustomerData _customerData;

        public AddCustomer(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public CustomerDto Execute(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _customerData.AddCustomer(customer);

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