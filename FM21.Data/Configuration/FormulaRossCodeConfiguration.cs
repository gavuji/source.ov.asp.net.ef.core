using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaRossCodeConfiguration : IEntityTypeConfiguration<FormulaRossCode>
    {
        public void Configure(EntityTypeBuilder<FormulaRossCode> builder)
        {
            builder.ToTable("FormulaRossCodeID");
            builder.HasKey(e => e.FormulaRossCodeID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.FormulaRossCode)
                .HasForeignKey(d => d.SiteID)
                .HasConstraintName("FK_FormulaRossCode_SiteMaster");
        }
    }
}