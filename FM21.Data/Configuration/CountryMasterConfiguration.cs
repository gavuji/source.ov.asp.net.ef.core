using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class CountryMasterConfiguration : IEntityTypeConfiguration<CountryMaster>
    {
        public void Configure(EntityTypeBuilder<CountryMaster> builder)
        {
            builder.ToTable("CountryMaster");
            builder.HasKey(e => e.CountryID);

            builder.Property(e => e.CountryName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}