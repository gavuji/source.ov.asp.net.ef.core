using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class BrokerMasterConfiguration : IEntityTypeConfiguration<BrokerMaster>
    {
        public void Configure(EntityTypeBuilder<BrokerMaster> builder)
        {
            builder.ToTable("BrokerMaster");
            builder.HasKey(e => e.BrokerID);

            builder.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.BrokerName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}