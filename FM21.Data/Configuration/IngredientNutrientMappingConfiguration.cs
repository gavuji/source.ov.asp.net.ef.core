using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientNutrientMappingConfiguration : IEntityTypeConfiguration<IngredientNutrientMapping>
    {
        public void Configure(EntityTypeBuilder<IngredientNutrientMapping> builder)
        {
            builder.ToTable("IngredientNutrientMapping");
            builder.HasKey(e => e.IngredientNutrientMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.Ingredient)
                .WithMany(p => p.IngredientNutrientMapping)
                .HasForeignKey(d => d.IngredientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientNutrientMapping_IngredientMaster");

            builder.HasOne(d => d.Nutrient)
                .WithMany(p => p.IngredientNutrientMapping)
                .HasForeignKey(d => d.NutrientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientNutrientMapping_NutrientMaster");
        }
    }
}