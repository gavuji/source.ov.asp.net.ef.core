using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FM21.Data
{
    public class RolePermissionMappingConfiguration : IEntityTypeConfiguration<RolePermissionMapping>
    {
        public void Configure(EntityTypeBuilder<RolePermissionMapping> builder)
        {
            builder.ToTable("RolePermissionMapping");

            builder.HasKey(e => e.RolePermissionID)
                   .HasName("PK_RolePermission");

            builder.Property(e => e.RolePermissionID).HasColumnName("RolePermissionID");

            builder.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.PermissionID).HasColumnName("PermissionID");

            builder.Property(e => e.RoleID).HasColumnName("RoleID");

            builder.Property(e => e.UpdatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissionMapping)
                .HasForeignKey(d => d.PermissionID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissionPermissionMaster");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.RolePermissionMapping)
                .HasForeignKey(d => d.RoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermissionRoleMaster");
        }
    }
}