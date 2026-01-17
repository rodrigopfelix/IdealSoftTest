namespace IdealSoftTestWPFClient.Models
{
    public interface ITokenStore
    {
        string? AccessToken { get; set; }
        void Clear();
    }
}
