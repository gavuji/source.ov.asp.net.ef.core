using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class RegulatoryMasterConfiguration : IEntityTypeConfiguration<RegulatoryMaster>
    {
        public void Configure(EntityTypeBuilder<RegulatoryMaster> builder)
        {
            builder.ToTable("RegulatoryMaster");
            builder.HasKey(k => k.RegulatoryId);

            builder.Property(e => e.NutrientId);
            builder.Property(e => e.UnitPerMg)
              .IsRequired();
            builder.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.NutrientMaster)
              .WithMany(p => p.RegulatoryMaster)
              .HasForeignKey(d => d.NutrientId)
              .HasConstraintName("FK_NutrientMaster");
        }
    }
}