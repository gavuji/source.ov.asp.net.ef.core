using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class PowderUnitServingSiteMappingConfiguration : IEntityTypeConfiguration<PowderUnitServingSiteMapping>
    {
        public void Configure(EntityTypeBuilder<PowderUnitServingSiteMapping> builder)
        {
            builder.ToTable("PowderUnitServingSiteMapping");
            builder.HasKey(e => e.PowderUnitServingSiteMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.UnitServingMaster)
                .WithMany(p => p.PowderUnitServingSiteMapping)
                .HasForeignKey(d => d.PowderUnitServingID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PowderUnitServingSiteMapping_UnitServingMaster");

            builder.HasOne(d => d.SiteProductMap)
                .WithMany(p => p.PowderUnitServingSiteMapping)
                .HasForeignKey(d => d.SiteProductMapID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PowderUnitServingSiteMapping_SiteProductTypeMapping");
        }
    }
}