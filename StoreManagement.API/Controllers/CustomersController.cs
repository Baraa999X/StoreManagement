using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Customers;
using StoreManagement.Application.DTOs;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CustomersController : ControllerBase
    {
        private readonly AddCustomer _addCustomer;
        private readonly GetAllCustomers _getAllCustomers;
        private readonly GetCustomerById _getCustomerById;
        private readonly UpdateCustomer _updateCustomer;
        private readonly DeleteCustomer _deleteCustomer;

        public CustomersController(AddCustomer addCustomer,GetAllCustomers getAllCustomers,GetCustomerById getCustomerById,UpdateCustomer updateCustomer,DeleteCustomer deleteCustomer)
        {
            _addCustomer = addCustomer;
            _getAllCustomers = getAllCustomers;
            _getCustomerById = getCustomerById;
            _updateCustomer = updateCustomer;
            _deleteCustomer = deleteCustomer;
        }

        [HttpPost]
        public IActionResult Add(CreateCustomerDto dto)
        {
            var addedCustomer = _addCustomer.Execute(dto);
            return CreatedAtAction(nameof(GetById), new { id = addedCustomer.Id }, addedCustomer);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_getAllCustomers.Execute());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _getCustomerById.Execute(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCustomerDto dto)
        {
            var updatedCustomer = _updateCustomer.Execute(id, dto);

            if (updatedCustomer == null)
            {
                return NotFound();
            }

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedCustomer = await _deleteCustomer.ExecuteAsync(id);

            if (deletedCustomer == null)
            {
                return NotFound();
            }

            return Ok(deletedCustomer);
        }
    }
}