using IdealSoftTestServer.Application.Interfaces;
using IdealSoftTestServer.Domain.Entities;
using IdealSoftTestServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdealSoftTestServer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateCustomerAsync(string firstName, string lastName)
        {
            var customer = new Customer(firstName, lastName);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync(int page, int pageSize)
        {
            return await _context.Customers
                .Where(c => c.DeletedAt == null)
                .Include(c => c.Phones.Where(p => p.DeletedAt == null))
                .OrderBy(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid customerId)
        {
            return await _context.Customers
                .Where(c => c.DeletedAt == null)
                .Include(c => c.Phones.Where(p => p.DeletedAt == null))
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<Customer?> UpdateCustomerAsync(Guid customerId, string firstName, string lastName)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer not found.");

            customer.Update(firstName, lastName);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> DeleteCustomerAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Customer not found.");

            //_context.Customers.Remove(customer);
            customer.Delete();
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
