using IdealSoftTestWPFClient.Models;

namespace IdealSoftTestWPFClient.Services
{
    public interface IPhoneService
    {
        Task<IList<Phone>> GetAllByCustomerIdAsync(Guid costumerId);

        Task<Phone?> GetByIdAsync(Guid costumerId, Guid phoneId);

        Task DeleteAsync(Guid customerId, Guid phoneId);

        Task UpdateAsync(Guid customerId, Phone phone);

        Task<Phone> CreateForCustomerAsync(Guid customerId, Phone phone);
    }
}
