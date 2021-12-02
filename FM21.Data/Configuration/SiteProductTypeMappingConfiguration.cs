using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SiteProductTypeMappingConfiguration : IEntityTypeConfiguration<SiteProductTypeMapping>
    {
        public void Configure(EntityTypeBuilder<SiteProductTypeMapping> builder)
        {
            builder.ToTable("SiteProductTypeMapping");
            builder.HasKey(e => e.SiteProductMapID);

            builder.HasOne(d => d.ProductType)
                .WithMany(p => p.SiteProductTypeMapping)
                .HasForeignKey(d => d.ProductTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiteProductTypeMapping_ProductTypeMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.SiteProductTypeMapping)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiteProductTypeMapping_SiteMaster");
        }
    }
}
