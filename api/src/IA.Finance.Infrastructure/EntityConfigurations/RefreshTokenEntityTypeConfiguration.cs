using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IA.Finance.Infrastructure.EntityConfigurations
{
    public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable($"{nameof(RefreshToken)}s");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired(false);

            builder.Ignore(e => e.Active);

            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Token).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Expires).IsRequired();
            builder.Property(e => e.RemoteIpAddress).HasMaxLength(255).IsRequired(false);
        }
    }
}