using IdealSoftTestServer.Domain.Entities;

namespace IdealSoftTestServer.Application.Interfaces
{
    public interface IPhoneService
    {
        public Task<List<Phone>> GetPhonesByCustomerIdAsync(Guid customerId);

        public Task<Phone?> GetPhoneByIdAsync(Guid phoneId);

        public Task<Phone> DeletePhoneAsync(Guid phoneId);

        public Task<Phone> UpdatePhoneAsync(Guid phoneId, string regionCode, string number, string type);

        public Task<Phone> CreatePhoneForCustomerAsync(Guid customerId, string regionCode, string number, string type);
    }
}
