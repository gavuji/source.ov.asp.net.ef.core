using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FM21.Data
{

    public class FormulaRevisionConfiguration : IEntityTypeConfiguration<FormulaRevision>
    {
        public void Configure(EntityTypeBuilder<FormulaRevision> builder)
        {
            builder.ToTable("FormulaRevision");
            builder.HasKey(e => e.FormulaRevisionID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
            builder.HasOne(d => d.FormulaMaster
            )
                .WithMany(p => p.FormulaRevision)
                .HasForeignKey(d => d.FormulaID)
                .HasConstraintName("FK_FormulaRevision_FormulaRevision");
        }
    }
}