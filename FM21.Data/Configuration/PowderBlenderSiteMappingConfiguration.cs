using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class PowderBlenderSiteMappingConfiguration : IEntityTypeConfiguration<PowderBlenderSiteMapping>
    {
        public void Configure(EntityTypeBuilder<PowderBlenderSiteMapping> builder)
        {
            builder.ToTable("PowderBlenderSiteMapping");
            builder.HasKey(e => e.PowderBlenderSiteMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.PowderBlender)
                .WithMany(p => p.PowderBlenderSiteMapping)
                .HasForeignKey(d => d.PowderBlenderID)
                .HasConstraintName("FK_PowderBlenderSiteMapping_PowderBlenderMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.PowderBlenderSiteMapping)
                .HasForeignKey(d => d.SiteID)
                .HasConstraintName("FK_PowderBlenderSiteMapping_SiteMaster");
        }
    }
}