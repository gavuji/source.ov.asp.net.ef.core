using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaChangeCodeConfiguration : IEntityTypeConfiguration<FormulaChangeCode>
    {
        public void Configure(EntityTypeBuilder<FormulaChangeCode> builder)
        {
            builder.ToTable("FormulaChangeCode");
            builder.HasKey(e => e.FormulaChangeCodeID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");

            builder.HasOne(d => d.FormulaTypeMaster)
                .WithMany(p => p.FormulaChangeCode)
                .HasForeignKey(d => d.FormulaTypeID)
                .HasConstraintName("FK_FormulaChangeCode_FormulaTypeMaster");

            builder.HasOne(d => d.Site)
                .WithMany(p => p.FormulaChangeCode)
                .HasForeignKey(d => d.SiteID)
                .HasConstraintName("FK_FormulaChangeCode_SiteMaster");
        }
    }
}