using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaProductionLineMappingConfiguration : IEntityTypeConfiguration<FormulaProductionLineMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaProductionLineMapping> builder)
        {
            builder.ToTable("FormulaProductionLineMapping");
            builder.HasKey(e => e.FormulaProductionLineMapID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.Formula)
                .WithMany(p => p.FormulaProductionLineMapping)
                .HasForeignKey(d => d.FormulaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaProductionLineMapping_FormulaMaster");

            builder.HasOne(d => d.ProductionLine)
                .WithMany(p => p.FormulaProductionLineMapping)
                .HasForeignKey(d => d.ProductionLineID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaProductionLineMapping_ProductionLineMaster");

            builder.HasOne(d => d.ProductionMixer)
                .WithMany(p => p.FormulaProductionLineMapping)
                .HasForeignKey(d => d.ProductionMixerID)
                .HasConstraintName("FK_FormulaProductionLineMapping_ProductionMixerMaster");
        }
    }
}