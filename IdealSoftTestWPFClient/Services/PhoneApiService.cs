using IdealSoftTestWPFClient.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace IdealSoftTestWPFClient.Services
{
    public class PhoneApiService : IPhoneService
    {
        private readonly HttpClient _http;

        public PhoneApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IList<Phone>> GetAllByCustomerIdAsync(Guid customerId)
            => await _http.GetFromJsonAsync<List<Phone>>($"api/customers/{customerId}/phones") ?? new();

        public async Task<Phone> GetByIdAsync(Guid customerId, Guid id)
            => await _http.GetFromJsonAsync<Phone>($"api/customers/{customerId}/phones/{id}") ?? new();

        public async Task<Phone> CreateForCustomerAsync(Guid customerId, Phone phone)
        {
            var response = await _http.PostAsJsonAsync($"api/customers/{customerId}/phones", phone);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Phone>() ?? new();
        }

        public async Task UpdateAsync(Guid customerId, Phone phone)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/customers/{customerId}/phones/{phone.Id}", phone);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid customerId, Guid phoneId)
        {
            var response = await _http.DeleteAsync($"api/customers/{customerId}/phones/{phoneId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
