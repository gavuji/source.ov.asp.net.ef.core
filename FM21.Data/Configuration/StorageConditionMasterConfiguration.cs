using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class StorageConditionMasterConfiguration : IEntityTypeConfiguration<StorageConditionMaster>
    {
        public void Configure(EntityTypeBuilder<StorageConditionMaster> builder)
        {
            builder.ToTable("StorageConditionMaster");
            builder.HasKey(e => e.StorageConditionID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.StorageDescription)
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.StorageType)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedOn).HasColumnType("datetime");
        }
    }
}