namespace IdealSoftTestWPFClient.Models
{
    public class InMemoryTokenStore : ITokenStore
    {
        public string? AccessToken { get; set; }
        public void Clear() => AccessToken = null;
    }
}
