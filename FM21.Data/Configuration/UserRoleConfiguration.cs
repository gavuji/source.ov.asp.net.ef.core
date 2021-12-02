using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(k => k.UserRoleID);

            builder.Property(e => e.UserID);

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.RoleID);

            builder.HasOne(d => d.Role)
                .WithMany(p => p.UserRole)
                .HasForeignKey(d => d.RoleID)
                .HasConstraintName("FK_UserRolePermissionMaster");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserRole)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK_UserRoleRoleMaster");
        }
    }
}