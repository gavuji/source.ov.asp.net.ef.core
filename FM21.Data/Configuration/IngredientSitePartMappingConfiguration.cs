using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientSitePartMappingConfiguration : IEntityTypeConfiguration<IngredientSitePartMapping>
    {
        public void Configure(EntityTypeBuilder<IngredientSitePartMapping> builder)
        {
            builder.ToTable("IngredientSitePartMapping");
            builder.HasKey(e => e.IngredientSitePartMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.PartNumber)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.Ingredient)
                .WithMany(p => p.IngredientSitePartMapping)
                .HasForeignKey(d => d.IngredientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientSitePartMapping_IngredientMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.IngredientSitePartMapping)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientSitePartMapping_SiteMaster");
        }
    }
}