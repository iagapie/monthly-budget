using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IA.Finance.Api.Identity
{
    public class AppIdentityDbContext : IdentityDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>().HasData(new
            {
                Id = "d48ef9ed-313e-40d5-94f8-6fb1d0b20d3d",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@finance.com",
                NormalizedEmail = "ADMIN@FINANCE.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAEAACcQAAAAEFxH2wykvZDg+IuZsiD3qMuTagEKd73JG4lNDzzRhLN3vYsUrTqrCh0/IQgO+qVhAg==", // Admin123
                SecurityStamp = "N2LVMJFJEXOV24XLKLO7P3EMAO66JSAZ",
                ConcurrencyStamp = "4c3d05fe-8792-4dc0-87b7-ca27b334d262",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0
            });
        }
    }
}