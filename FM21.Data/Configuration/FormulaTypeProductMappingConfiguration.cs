using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaTypeProductMappingConfiguration : IEntityTypeConfiguration<FormulaTypeProductMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaTypeProductMapping> builder)
        {
            builder.ToTable("FormulaTypeProductMapping");
            builder.HasKey(e => e.FormulaTypeProductMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.FormulaType)
                .WithMany(p => p.FormulaTypeProductMapping)
                .HasForeignKey(d => d.FormulaTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaTypeProductMapping_FormulaTypeMaster");

            builder.HasOne(d => d.Product)
                .WithMany(p => p.FormulaTypeProductMapping)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaTypeProductMapping_ProductTypeMaster");
        }
    }
}