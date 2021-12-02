using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class PermissionMasterConfiguration : IEntityTypeConfiguration<PermissionMaster>
    {
        public void Configure(EntityTypeBuilder<PermissionMaster> builder)
        {
            builder.ToTable("PermissionMaster");
            builder.HasKey(e => e.PermissionID);

            builder.Property(e => e.PermissionID).HasColumnName("PermissionID");

            builder.Property(e => e.PermissionFor)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn)
               .HasColumnType("datetime")
               .HasDefaultValueSql("(getdate())");
        }
    }
}