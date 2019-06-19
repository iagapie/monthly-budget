using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IA.Finance.Infrastructure.EntityConfigurations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable($"{nameof(Project)}s");
                
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Currency).HasMaxLength(255).IsRequired();

            builder.HasIndex(nameof(Project.Name));
            builder.HasIndex(nameof(Project.Currency));
            
            builder.Property(e => e.OwnerId).IsRequired();

            builder.Property(e => e.CreatedAt).IsRequired();
            
            builder.Property(e => e.UpdatedAt).IsRequired(false);
            
            builder.HasOne<User>().WithMany().HasForeignKey(e => e.OwnerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}