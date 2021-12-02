using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaClaimMappingConfiguration : IEntityTypeConfiguration<FormulaClaimMapping>
    {
        public void Configure(EntityTypeBuilder<FormulaClaimMapping> builder)
        {
            builder.ToTable("FormulaClaimMapping");
            builder.HasKey(e => e.FormulaClaimMapID);

            builder.Property(e => e.Description)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.ClaimMaster)
                .WithMany(p => p.FormulaClaimMapping)
                .HasForeignKey(d => d.ClaimID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaClaimMappingClaimMaster");

            builder.HasOne(d => d.FormulaMaster)
                .WithMany(p => p.FormulaClaimMapping)
                .HasForeignKey(d => d.FormulaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaClaimMappingFormulaMaster");
        }
    }
}