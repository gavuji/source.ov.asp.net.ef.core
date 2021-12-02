using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class KosherCodeMasterConfiguration : IEntityTypeConfiguration<KosherCodeMaster>
    {
        public void Configure(EntityTypeBuilder<KosherCodeMaster> builder)
        {
            builder.ToTable("KosherCodeMaster");
            builder.HasKey(e => e.KosherCodeID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.KosherCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.KosherCodeDescription)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}