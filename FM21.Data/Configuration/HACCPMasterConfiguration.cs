using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class HACCPMasterConfiguration : IEntityTypeConfiguration<HACCPMaster>
    {
        public void Configure(EntityTypeBuilder<HACCPMaster> builder)
        {
            builder.ToTable("HACCPMaster");
            builder.HasKey(e => e.HACCPID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.HACCPDescription)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.HACCPType)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("((0))");
        }
    }
}