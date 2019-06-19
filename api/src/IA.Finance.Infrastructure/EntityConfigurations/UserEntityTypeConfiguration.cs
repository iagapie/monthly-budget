using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IA.Finance.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{nameof(User)}s");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired(false);
            
            builder.Property(e => e.Id).IsRequired().HasDefaultValueSql("nextval('\"AppUserId\"')");;
            
            builder.Property(e => e.UserName).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(255).IsRequired();
            builder.Property(e => e.IdentityId).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Role).HasMaxLength(255).IsRequired();
            builder.Property(e => e.FirstName).HasMaxLength(255).IsRequired(false);
            builder.Property(e => e.LastName).HasMaxLength(255).IsRequired(false);
            
            var navigation = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));
            
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.HasIndex(e => e.IdentityId).IsUnique();
            builder.HasIndex(e => e.UserName).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}