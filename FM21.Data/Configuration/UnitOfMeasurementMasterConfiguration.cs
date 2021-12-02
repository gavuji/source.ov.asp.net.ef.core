using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class UnitOfMeasurementMasterConfiguration : IEntityTypeConfiguration<UnitOfMeasurementMaster>
    {
        public void Configure(EntityTypeBuilder<UnitOfMeasurementMaster> builder)
        {
            builder.ToTable("UnitOfMeasurementMaster");
            builder.HasKey(e => e.UnitOfMeasurementID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.MeasurementUnit)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}