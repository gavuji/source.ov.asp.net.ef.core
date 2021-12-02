using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaDetailMappingConfiguration : IEntityTypeConfiguration<FormulaDetailMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaDetailMapping> builder)
        {
            builder.ToTable("FormulaDetailMapping");
            builder.HasKey(e => e.FormulaDetailMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.Formula)
                .WithMany(p => p.FormulaDetailMapping)
                .HasForeignKey(d => d.FormulaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaDetailMapping_FormulaMaster");
        }
    }
}