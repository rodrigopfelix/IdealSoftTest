using IdealSoftTestServer.Application.Interfaces;
using IdealSoftTestServer.Domain.Entities;
using IdealSoftTestServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdealSoftTestServer.Application.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly AppDbContext _context;

        public PhoneService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Phone>> GetPhonesByCustomerIdAsync(Guid customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == customerId)
                ?? throw new KeyNotFoundException("Customer not found.");

            return customer.Phones
                .Where(p => p.DeletedAt == null)
                .OrderBy(p => p.CreatedAt)
                .ToList();
        }

        public async Task<Phone?> GetPhoneByIdAsync(Guid phoneId)
        {
            return await _context.Phones.FindAsync(phoneId);
        }

        public async Task<Phone> DeletePhoneAsync(Guid phoneId)
        {
            var phone = await _context.Phones.FindAsync(phoneId)
                ?? throw new KeyNotFoundException("Phone not found.");
            //_context.Phones.Remove(phone);
            phone.Delete();
            await _context.SaveChangesAsync();
            return phone;
        }

        public async Task<Phone> UpdatePhoneAsync(Guid phoneId, string regionCode, string number, string type)
        {
            var phone = await _context.Phones.FindAsync(phoneId)
                ?? throw new KeyNotFoundException("Phone not found.");

            phone.Update(number, regionCode, type);
            await _context.SaveChangesAsync();
            return phone;
        }

        public async Task<Phone> CreatePhoneForCustomerAsync(Guid customerId, string regionCode, string number, string type)
        {
            var customer = await _context.Customers
                .Include(c => c.Phones)
                .FirstOrDefaultAsync(c => c.Id == customerId)
                ?? throw new KeyNotFoundException("Customer not found.");

            var phone = new Phone(number, regionCode, type);
            _context.Phones.Add(phone);
            customer.AddPhone(phone);
            await _context.SaveChangesAsync();
            return phone;
        }
    }
}
