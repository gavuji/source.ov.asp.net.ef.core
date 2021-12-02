using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class InstructionGroupMasterConfiguration : IEntityTypeConfiguration<InstructionGroupMaster>
    {
        public void Configure(EntityTypeBuilder<InstructionGroupMaster> builder)
        {
            builder.ToTable("InstructionGroupMaster");
            builder.HasKey(k => k.InstructionGroupID);

            builder.Property(e => e.InstructionGroupName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.IsActive).HasDefaultValueSql("(1)");

            builder.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
        }
    }
}