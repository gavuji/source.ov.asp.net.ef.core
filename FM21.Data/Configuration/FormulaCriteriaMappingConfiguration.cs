using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaCriteriaMappingConfiguration : IEntityTypeConfiguration<FormulaCriteriaMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaCriteriaMapping> builder)
        {
            builder.ToTable("FormulaCriteriaMapping");
            builder.HasKey(e => e.FormulaCriteriaMapID);

            builder.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.CriteriaMaster)
                .WithMany(p => p.FormulaCriteriaMapping)
                .HasForeignKey(d => d.CriteriaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaCriteriaMappingCriteriaMaster");

            builder.HasOne(d => d.FormulaMaster)
                .WithMany(p => p.FormulaCriteriaMapping)
                .HasForeignKey(d => d.FormulaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaCriteriaMappingFormulaMaster");
        }
    }
}