using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{

    public class ProjectMasterConfiguration : IEntityTypeConfiguration<ProjectMaster>
    {
        public void Configure(EntityTypeBuilder<ProjectMaster> builder)
        {
            builder.ToTable("ProjectMaster");
            builder.HasKey(k => k.ProjectId);

            builder.Property(e => e.CustomerId);
            builder.Property(e => e.ProjectCode)
               .IsRequired();
            builder.Property(e => e.ProjectDescription)
              .IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.CustomerMaster)
              .WithMany(p => p.ProjectMaster)
              .HasForeignKey(d => d.CustomerId)
              .HasConstraintName("FK_ProjectMaster_Customer");
        }
    }
}
