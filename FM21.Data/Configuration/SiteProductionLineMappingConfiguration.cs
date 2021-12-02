using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SiteProductionLineMappingConfiguration : IEntityTypeConfiguration<SiteProductionLineMapping>
    {
        public void Configure(EntityTypeBuilder<SiteProductionLineMapping> builder)
        {
            builder.ToTable("SiteProductionLineMapping");
            builder.HasKey(e => e.SiteProductionLineMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.ProductionLine)
                .WithMany(p => p.SiteProductionLineMapping)
                .HasForeignKey(d => d.ProductionLineID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiteProductionLineMapping_ProductionLineMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.SiteProductionLineMapping)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiteProductionLineMapping_SiteMaster");
        }
    }
}