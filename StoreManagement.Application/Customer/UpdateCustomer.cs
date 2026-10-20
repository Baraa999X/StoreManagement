using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Customers
{
    public class UpdateCustomer
    {
        private readonly ICustomerData _customerData;

        public UpdateCustomer(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public CustomerDto? Execute(int id, UpdateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var updatedCustomer = _customerData.UpdateCustomer(id, customer);

            if (updatedCustomer == null)
            {
                return null;
            }

            var customerDto = new CustomerDto
            {
                Id = updatedCustomer.Id,
                Name = updatedCustomer.Name,
                Email = updatedCustomer.Email,
                Phone = updatedCustomer.Phone
            };

            return customerDto;
        }
    }
}