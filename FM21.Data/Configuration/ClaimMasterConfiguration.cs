using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class ClaimMasterConfiguration : IEntityTypeConfiguration<ClaimMaster>
    {
        public void Configure(EntityTypeBuilder<ClaimMaster> builder)
        {
            builder.ToTable("ClaimMaster");
            builder.HasKey(e => e.ClaimID);

            builder.Property(e => e.ClaimCode)
                     .HasMaxLength(20)
                     .IsUnicode(false);

            builder.Property(e => e.ClaimDescription)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.ClaimGroupType)
                .HasMaxLength(20)
                .IsUnicode(false);


            builder.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}