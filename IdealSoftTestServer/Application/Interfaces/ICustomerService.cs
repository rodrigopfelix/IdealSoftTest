using IdealSoftTestServer.Domain.Entities;

namespace IdealSoftTestServer.Application.Interfaces
{
    public interface ICustomerService
    {
        public Task<Customer> CreateCustomerAsync(string firstName, string lastName);

        public Task<List<Customer>> GetAllCustomersAsync(int page, int pageSize);

        public Task<Customer?> GetCustomerByIdAsync(Guid customerId);

        public Task<Customer?> UpdateCustomerAsync(Guid customerId, string firstName, string lastName);

        public Task<Customer?> DeleteCustomerAsync(Guid customerId);
    }
}
