using IdealSoftTestServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdealSoftTestServer.Infrastructure.Persistence
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Phone> Phones => Set<Phone>();
        //public DbSet<PhoneType> PhoneTypes => Set<PhoneType>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
