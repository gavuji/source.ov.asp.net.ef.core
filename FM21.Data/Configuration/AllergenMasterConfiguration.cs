using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class AllergenMasterConfiguration : IEntityTypeConfiguration<AllergenMaster>
    {
        public void Configure(EntityTypeBuilder<AllergenMaster> builder)
        {
            builder.ToTable("AllergenMaster");
            builder.HasKey(k => k.AllergenID);
            builder.Property(e => e.AllergenCode).IsRequired().HasMaxLength(10).IsUnicode(false);
            builder.Property(e => e.AllergenName).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.AllergenDescription_En).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.AllergenDescription_Fr).HasMaxLength(100);

            builder.Property(e => e.AllergenDescription_Es).HasMaxLength(100);

            builder.Property(e => e.CreatedOn)
               .HasColumnType("datetime")
               .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValueSql("(0)");

            builder.Property(e => e.IsUSAAllergen)
                .HasColumnType("bit")
                .HasDefaultValueSql("(0)");

            builder.Property(e => e.IsCANADAAllergen)
                .HasColumnType("bit")
                .HasDefaultValueSql("(0)");
        }
    }
}