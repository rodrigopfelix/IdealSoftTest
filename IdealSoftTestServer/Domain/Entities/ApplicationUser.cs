using Microsoft.AspNetCore.Identity;

namespace IdealSoftTestServer.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
    }
}
