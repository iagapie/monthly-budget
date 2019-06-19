using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IA.Finance.Infrastructure.EntityConfigurations
{
    public class MovementEntityTypeConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.ToTable($"{nameof(Movement)}s");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
            
            builder.Property(e => e.DirectionId).IsRequired();

            builder.Property(e => e.ProjectId).IsRequired();
            
            builder.Property(e => e.CreatedAt).IsRequired();
            
            builder.Property(e => e.UpdatedAt).IsRequired(false);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.PlanAmount).IsRequired();

            builder.Property(e => e.OwnerId).IsRequired(false);
            
            var navigation = builder.Metadata.FindNavigation(nameof(Movement.MovementItems));
            
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(e => e.Direction).WithMany().HasForeignKey(e => e.DirectionId);

            builder.HasOne<Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne<User>().WithMany().HasForeignKey(e => e.OwnerId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}