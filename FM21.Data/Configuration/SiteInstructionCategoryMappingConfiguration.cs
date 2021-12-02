using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SiteInstructionCategoryMappingConfiguration : IEntityTypeConfiguration<SiteInstructionCategoryMapping>
    {
        public void Configure(EntityTypeBuilder<SiteInstructionCategoryMapping> builder)
        {
            builder.ToTable("SiteInstructionCategoryMapping");
            builder.HasKey(k => k.SiteInstructionCategoryMapID);

            builder.HasOne(d => d.InstructionCategory)
                  .WithMany(p => p.SiteInstructionCategoryMapping)
                  .HasForeignKey(d => d.InstructionCategoryID)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_InstructionSiteMapInstructionCategory");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.SiteInstructionCategoryMapping)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InstructionSiteMapSiteMaster");
        }
    }
}