using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class RMStatusMasterConfiguration : IEntityTypeConfiguration<RMStatusMaster>
    {
        public void Configure(EntityTypeBuilder<RMStatusMaster> builder)
        {
            builder.ToTable("RMStatusMaster");
            builder.HasKey(e => e.RMStatusMasterID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.RMStatus)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.RMStatusType)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}