using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SiteMasterConfiguration : IEntityTypeConfiguration<SiteMaster>
    {
        public void Configure(EntityTypeBuilder<SiteMaster> builder)
        {
            builder.ToTable("SiteMaster");
            builder.HasKey(e => e.SiteID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.SiteCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.SiteDescription).HasMaxLength(250);
        }
    }
}