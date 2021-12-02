using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class CriteriaMasterConfiguration : IEntityTypeConfiguration<CriteriaMaster>
    {
        public void Configure(EntityTypeBuilder<CriteriaMaster> builder)
        {
            builder.ToTable("CriteriaMaster");
            builder.HasKey(e => e.CriteriaID);

            builder.Property(e => e.CriteriaDescription)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.ColorCode)
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}