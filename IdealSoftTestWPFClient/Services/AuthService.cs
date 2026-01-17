using IdealSoftTestWPFClient.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace IdealSoftTestWPFClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ITokenStore _tokenStore;

        public AuthService(HttpClient http, ITokenStore tokenStore)
        {
            _http = http;
            _tokenStore = tokenStore;
        }

        public async Task LoginAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync(
                "api/auth/login",
                new { email, password });

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<LoginResponse>();

            _tokenStore.AccessToken = result!.AccessToken;
        }

        public void Logout()
        {
            _tokenStore.Clear();
        }
    }

}
