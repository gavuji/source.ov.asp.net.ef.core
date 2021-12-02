using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaDatasheetMappingConfiguration : IEntityTypeConfiguration<FormulaDatasheetMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaDatasheetMapping> builder)
        {
            builder.ToTable("FormulaDatasheetMapping");
            builder.HasKey(e => e.FormulaDatasheetMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.DatasheetFormat)
                .WithMany(p => p.FormulaDatasheetMapping)
                .HasForeignKey(d => d.DatasheetFormatID)
                .HasConstraintName("FK_FormulaDatasheetMapping_DatasheetFormatMaster");

            builder.HasOne(d => d.Formula)
                .WithMany(p => p.FormulaDatasheetMapping)
                .HasForeignKey(d => d.FormulaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaDatasheetMapping_FormulaMaster");

            builder.HasOne(d => d.Nutrient)
                .WithMany(p => p.FormulaDatasheetMapping)
                .HasForeignKey(d => d.NutrientID)
                .HasConstraintName("FK_FormulaDatasheetMapping_NutrientMaster");
        }
    }
}