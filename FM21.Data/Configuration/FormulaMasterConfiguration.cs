using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class FormulaMasterConfiguration : IEntityTypeConfiguration<FormulaMaster>
    {
        public void Configure(EntityTypeBuilder<FormulaMaster> builder)
        {
            builder.ToTable("FormulaMaster");
            builder.HasKey(e => e.FormulaID);

            builder.Property(e => e.IsAllClaimVerify)
               .IsRequired()
               .HasDefaultValueSql("((0))");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.IsDeleted)
               .IsRequired()
               .HasDefaultValueSql("((0))");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.SiteProductMap)
                .WithMany(p => p.FormulaMaster)
                .HasForeignKey(d => d.SiteProductMapID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormulaMaster_SiteProductTypeMapping");

            builder.HasOne(d => d.SiteProductionLineMapping)
               .WithMany(p => p.FormulaMaster)
               .HasForeignKey(d => d.PrimaryProductionLineID)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_FormulaMaster_SiteProductionLineMapping");
        }
    }
}