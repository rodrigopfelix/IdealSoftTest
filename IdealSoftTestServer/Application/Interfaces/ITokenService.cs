using IdealSoftTestServer.Domain.Entities;

namespace IdealSoftTestServer.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
