using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Application.Contracts;
using StoreManagement.Domain.Entities;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Infrastructure.Identity;

namespace StoreManagement.Infrastructure.DataAccess
{
    public class CustomerData : ICustomerData
    {
        private readonly StoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerData(StoreDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ICollection<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer? GetCustomerById(int id) =>
            _context.Customers.FirstOrDefault(customer => customer.Id == id);

        public Customer AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;
        }

        public Customer? UpdateCustomer(int id, Customer customer)
        {
            var existingCustomer =
                _context.Customers.FirstOrDefault(c => c.Id == id);

            if (existingCustomer == null)
            {
                return null;
            }

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;

            _context.SaveChanges();

            return existingCustomer;
        }

        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (customer == null)
                {
                    return null;
                }

                var applicationUserId = customer.ApplicationUserId;

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(applicationUserId))
                {
                    var user = await _userManager
                        .FindByIdAsync(applicationUserId);

                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);

                        if (!result.Succeeded)
                        {
                            var errors = string.Join("; ", result.Errors.Select(error => error.Description));

                            throw new InvalidOperationException($"Failed to delete the customer's Identity account: {errors}");
                        }
                    }
                }

                await transaction.CommitAsync();

                return customer;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Customer AddCustomerForUser(
            Customer customer,
            string applicationUserId)
        {
            customer.ApplicationUserId = applicationUserId;

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }
    }
}