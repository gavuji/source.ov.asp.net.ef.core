using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class UserMasterConfiguration : IEntityTypeConfiguration<UserMaster>
    {
        public void Configure(EntityTypeBuilder<UserMaster> builder)
        {
            builder.ToTable("UserMaster");
            builder.HasKey(k => k.UserID);

            builder.Property(e => e.IsDeleted).HasDefaultValueSql("(0)");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}