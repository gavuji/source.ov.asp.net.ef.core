using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class ProductionLineMixerMappingConfiguration : IEntityTypeConfiguration<ProductionLineMixerMapping>
    {
        public void Configure(EntityTypeBuilder<ProductionLineMixerMapping> builder)
        {
            builder.ToTable("ProductionLineMixerMapping");
            builder.HasKey(e => e.ProductionLineMixerMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.ProductionMixerMaster)
                .WithMany(p => p.ProductionLineMixerMapping)
                .HasForeignKey(d => d.ProductionMixerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductionLineMixerMapping_ProductionMixerMaster");

            builder.HasOne(d => d.SiteProductionLine)
                .WithMany(p => p.ProductionLineMixerMapping)
                .HasForeignKey(d => d.SiteProductionLineID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductionLineMixerMapping_SiteProductionLineMapping");
        }
    }
}