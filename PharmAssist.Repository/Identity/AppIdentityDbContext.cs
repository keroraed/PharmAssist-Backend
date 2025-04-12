using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmAssist.Core.Entities.Identity;
using PharmAssist.Core.Entities.OTP;

namespace PharmAssist.Repository.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }
        public DbSet<OtpEntry> OtpEntries { get; set; }
    }
}
