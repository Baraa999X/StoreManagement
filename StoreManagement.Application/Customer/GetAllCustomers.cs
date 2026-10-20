using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Customers
{
    public class GetAllCustomers
    {
        private readonly ICustomerData _customerData;

        public GetAllCustomers(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public ICollection<CustomerDto> Execute()
        {
            var customers = _customerData.GetAllCustomers();

            var customerDtos = customers.Select(customer => new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            }).ToList();

            return customerDtos;
        }
    }
}