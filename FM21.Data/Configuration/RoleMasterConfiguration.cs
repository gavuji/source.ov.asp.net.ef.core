using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class RoleMasterConfiguration : IEntityTypeConfiguration<RoleMaster>
    {
        public void Configure(EntityTypeBuilder<RoleMaster> builder)
        {
            builder.ToTable("RoleMaster");
            builder.HasKey(k => k.RoleID);

            builder.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.RoleDescription)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.IsActive).HasDefaultValueSql("(1)");
            builder.Property(e => e.IsDeleted).HasDefaultValueSql("(0)");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}