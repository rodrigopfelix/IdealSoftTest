using IdealSoftTestWPFClient.Models;

namespace IdealSoftTestWPFClient.Services
{
    public interface ICustomerService
    {
        Task<IList<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
    }
}
