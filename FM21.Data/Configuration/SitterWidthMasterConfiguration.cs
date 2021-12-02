using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SitterWidthMasterConfiguration : IEntityTypeConfiguration<SitterWidthMaster>
    {
        public void Configure(EntityTypeBuilder<SitterWidthMaster> builder)
        {
            builder.ToTable("SitterWidthMaster");
            builder.HasKey(k => k.SitterWidthID);

            builder.HasOne(d => d.Site)
                .WithMany(p => p.SitterWidthMaster)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SitterWidthMaster_SiteMaster");
        }
    }
}