using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class IngredientCategoryMasterConfiguration : IEntityTypeConfiguration<IngredientCategoryMaster>
    {
        public void Configure(EntityTypeBuilder<IngredientCategoryMaster> builder)
        {
            builder.ToTable("IngredientCategoryMaster");
            builder.HasKey(e => e.IngredientCategoryID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IngredientCategoryCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.IngredientCategoryDescription)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.IngredientCategoryGeneralDescription)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}