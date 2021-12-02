using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientAllergenMappingConfiguration : IEntityTypeConfiguration<IngredientAllergenMapping>
    {
        public void Configure(EntityTypeBuilder<IngredientAllergenMapping> builder)
        {
            builder.ToTable("IngredientAllergenMapping");
            builder.HasKey(e => e.IngredientAllergenMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.Allergen)
                .WithMany(p => p.IngredientAllergenMapping)
                .HasForeignKey(d => d.AllergenID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientAllergenMapping_AllergenMaster");

            builder.HasOne(d => d.Ingredient)
                .WithMany(p => p.IngredientAllergenMapping)
                .HasForeignKey(d => d.IngredientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientAllergenMapping_IngredientMaster");
        }
    }
}