using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SiteProcessCodeConfiguration : IEntityTypeConfiguration<SiteProcessCode>
    {
        public void Configure(EntityTypeBuilder<SiteProcessCode> builder)
        {
            builder.ToTable("SiteProcessCode");
            builder.HasKey(e => e.SiteProcessCodeID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
            builder.HasOne(d => d.Site)
                .WithMany(p => p.SiteProcessCode)
                .HasForeignKey(d => d.SiteID)
                .HasConstraintName("FK_SiteProcessCode_SiteMaster");
        }
    }
}
