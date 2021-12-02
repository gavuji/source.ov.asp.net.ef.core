using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class SupplierMasterConfiguration: IEntityTypeConfiguration<SupplierMaster>
    {
        public void Configure(EntityTypeBuilder<SupplierMaster> builder)
        {
            builder.ToTable("SupplierMaster");
            builder.HasKey(k => k.SupplierID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsDeleted)
                .HasColumnType("bit")
                .HasDefaultValueSql("(0)");

        }
    }
}