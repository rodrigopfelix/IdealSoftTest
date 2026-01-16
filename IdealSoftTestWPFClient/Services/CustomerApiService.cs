using IdealSoftTestWPFClient.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace IdealSoftTestWPFClient.Services
{
    public class CustomerApiService : ICustomerService
    {
        private readonly HttpClient _http;
        private readonly string _endpoint = "api/customers";

        public CustomerApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IList<Customer>> GetAllAsync()
            => await _http.GetFromJsonAsync<List<Customer>>(_endpoint) ?? new();

        public async Task<Customer> GetByIdAsync(Guid id
            ) => await _http.GetFromJsonAsync<Customer>($"{_endpoint}/{id}") ?? new();

        public async Task<Customer> CreateAsync(Customer customer)
        {
            var response = await _http.PostAsJsonAsync(_endpoint, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>() ?? new();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var response = await _http.PutAsJsonAsync(
                $"{_endpoint}/{customer.Id}", customer);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"{_endpoint}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
