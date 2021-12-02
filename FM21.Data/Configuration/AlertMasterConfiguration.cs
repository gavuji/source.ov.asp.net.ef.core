using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class AlertMasterConfiguration : IEntityTypeConfiguration<AlertMaster>
    {
        public void Configure(EntityTypeBuilder<AlertMaster> builder)
        {
            builder.ToTable("AlertMaster");
            builder.HasKey(e => e.AlertID);

            builder.Property(e => e.AlertCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.AlertDescription)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.AlertType)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}