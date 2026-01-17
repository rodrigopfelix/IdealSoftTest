namespace IdealSoftTestWPFClient.Services
{
    public interface IAuthService
    {
        Task LoginAsync(string email, string password);
        void Logout();
    }
}
